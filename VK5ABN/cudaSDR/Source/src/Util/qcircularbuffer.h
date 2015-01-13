/****************************************************************************
**
** Copyright (C) 2010 Nokia Corporation and/or its subsidiary(-ies).
** All rights reserved.
** Contact: Nokia Corporation (qt-info@nokia.com)
**
** This file is part of the QtCore module of the Qt Toolkit.
**
** $QT_BEGIN_LICENSE:LGPL$
** No Commercial Usage
** This file contains pre-release code and may not be distributed.
** You may use this file in accordance with the terms and conditions
** contained in the Technology Preview License Agreement accompanying
** this package.
**
** GNU Lesser General Public License Usage
** Alternatively, this file may be used under the terms of the GNU Lesser
** General Public License version 2.1 as published by the Free Software
** Foundation and appearing in the file LICENSE.LGPL included in the
** packaging of this file.  Please review the following information to
** ensure the GNU Lesser General Public License version 2.1 requirements
** will be met: http://www.gnu.org/licenses/old-licenses/lgpl-2.1.html.
**
** In addition, as a special exception, Nokia gives you certain additional
** rights.  These rights are described in the Nokia Qt LGPL Exception
** version 1.1, included in the file LGPL_EXCEPTION.txt in this package.
**
** If you have questions regarding the use of this file, please contact
** Nokia at qt-info@nokia.com.
**
**
**
**
**
**
**
**
** $QT_END_LICENSE$
**
****************************************************************************/

#ifndef QCIRCULARBUFFER_H
#define QCIRCULARBUFFER_H

#include <QtCore/qlist.h>
#include <QtCore/qpair.h>
#include <QtCore/qshareddata.h>
#include <QtCore/qvector.h>

QT_BEGIN_HEADER

QT_BEGIN_NAMESPACE

QT_MODULE(Core)

template <typename T>
class CircularBufferData : public QSharedData
{
public:
    CircularBufferData()
        : data(0),
          capacity(0),
          size(0),
          first(-1),
          last(-1)
    {}

    ~CircularBufferData()
    {
        // Cleanup the data first
        if (QTypeInfo<T>::isComplex) {
            // The type is complex so we manually call the destructor for each item
            // since we used the placement new operator to instantiate them
            T* b = data;
            T* i = b + capacity;
            while (i-- != b)
                 i->~T();
        }

        // Release the raw memory
        deallocate(data);
    }

    T* allocate(int capacity);
    void deallocate(T* data);

    T* data;      // Array of the actual data
    int capacity; // Size of the m_data array
    int size;     // Number of elements of m_data actually used
    int first;    // Index in m_data of the first element of the circular buffer
    int last;     // Index in m_data of the last element of the circular buffer
};

template <typename T>
T* CircularBufferData<T>::allocate(int capacity)
{
    void* rawMemory = operator new[](capacity * sizeof(T));
    T* data = static_cast<T*>(rawMemory);
    return data;
}

template <typename T>
void CircularBufferData<T>::deallocate(T* data)
{
    operator delete[](data);
}


template <typename T>
class QCircularBuffer
{
public:
    typedef QPair<T*,int> array_range;
    typedef QPair<const T*,int> const_array_range;
    typedef array_range ArrayRange;
    typedef const_array_range ConstArrayRange;

    QCircularBuffer()
        : d(new CircularBufferData<T>())
    {}

    explicit QCircularBuffer(int capacity);
    explicit QCircularBuffer(int capacity, const T &value);
    explicit QCircularBuffer(int capacity, int size, const T &value);

    QCircularBuffer(const QCircularBuffer<T>& other)
        : d(other.d)
    {}

    QCircularBuffer<T>& operator = (const QCircularBuffer<T>& other)
    {
        d = other.d;
        return *this;
    }

    ~QCircularBuffer() {}

    class iterator
    {
    public:
        typedef std::random_access_iterator_tag iterator_category;
        typedef ptrdiff_t difference_type;
        typedef T value_type;
        typedef T *pointer;
        typedef T &reference;

        iterator() : buffer(0), index(-1) {}
        iterator(QCircularBuffer<T>* buffer, int index)
            : buffer(buffer), index(index)
        {}
        iterator(const iterator& other)
            : buffer(other.buffer), index(other.index)
        {}

        T &operator*() const { return (*buffer)[ index ]; }
        T* operator->() const
        {
            return buffer->d->data + ((buffer->d->first + index) % buffer->d->capacity);
        }
        T &operator[](int j) const { return (*buffer)[ index + j ]; }

        bool operator==(const iterator& other) const
        {
            return (buffer == other.buffer && index == other.index);
        }

        bool operator!=(const iterator& other) const
        {
            return (buffer != other.buffer || index != other.index);
        }

        bool operator<(const iterator& other) const
        {
            Q_ASSERT_X(buffer == other.buffer, "QCircularBuffer<T>::iterator::operator<", "iterators use different buffers");
            return index < other.index;
        }

        bool operator<=(const iterator& other) const
        {
            Q_ASSERT_X(buffer == other.buffer, "QCircularBuffer<T>::iterator::operator<=", "iterators use different buffers");
            return index <= other.index;
        }

        bool operator>(const iterator& other) const
        {
            Q_ASSERT_X(buffer == other.buffer, "QCircularBuffer<T>::iterator::operator>", "iterators use different buffers");
            return index > other.index;
        }

        bool operator>=(const iterator& other) const
        {
            Q_ASSERT_X(buffer == other.buffer, "QCircularBuffer<T>::iterator::operator>=", "iterators use different buffers");
            return index >= other.index;
        }

        iterator& operator++() { ++index; return *this; }
        iterator operator++(int)
        {
            iterator ans = *this;
            ++index;
            return ans;
        }

        iterator& operator--() { --index; return *this; }
        iterator operator--(int)
        {
            iterator ans = *this;
            --index;
            return ans;
        }

        iterator& operator+=(int j) { index += j; return *this; }
        iterator& operator-=(int j) { index -= j; return *this; }
        iterator operator+(int j) const { return iterator(buffer, index + j); }
        iterator operator-(int j) const { return iterator(buffer, index - j); }
        int operator-(iterator other) const
        {
            Q_ASSERT_X(buffer == other.buffer, "QCircularBuffer<T>::iterator::operator-", "iterators use different buffers");
            return index - other.index;
        }

    private:
        QCircularBuffer<T>* buffer;
        int index;
        friend class QCircularBuffer;
    };
    friend class iterator;

    class const_iterator
    {
    public:
        typedef std::random_access_iterator_tag  iterator_category;
        typedef ptrdiff_t difference_type;
        typedef T value_type;
        typedef const T* pointer;
        typedef const T &reference;

        const_iterator() : buffer(0), index(-1) {}
        const_iterator(const QCircularBuffer<T>* buffer, int index)
            : buffer(buffer), index(index)
        {}
        const_iterator(const const_iterator& other)
            : buffer(other.buffer), index(other.index)
        {}
        explicit const_iterator(const iterator& other)
            : buffer(other.buffer), index(other.index)
        {}

        const T &operator*() const { return buffer->at(index); }
        const T* operator->() const
        {
            return buffer->d->data + ((buffer->d->first + index) % buffer->d->capacity);
        }
        const T &operator[](int j) const { return buffer->at(index + j); }

        bool operator==(const const_iterator& other) const
        {
            return (buffer == other.buffer && index == other.index);
        }

        bool operator!=(const const_iterator& other) const
        {
            return (buffer != other.buffer || index != other.index);
        }

        bool operator<(const const_iterator& other) const
        {
            Q_ASSERT_X(buffer == other.buffer, "QCircularBuffer<T>::const_iterator::operator<", "iterators use different buffers");
            return index < other.index;
        }

        bool operator<=(const const_iterator& other) const
        {
            Q_ASSERT_X(buffer == other.buffer, "QCircularBuffer<T>::const_iterator::operator<=", "iterators use different buffers");
            return index <= other.index;
        }

        bool operator>(const const_iterator& other) const
        {
            Q_ASSERT_X(buffer == other.buffer, "QCircularBuffer<T>::const_iterator::operator>", "iterators use different buffers");
            return index > other.index;
        }

        bool operator>=(const const_iterator& other) const
        {
            Q_ASSERT_X(buffer == other.buffer, "QCircularBuffer<T>::const_iterator::operator>=", "iterators use different buffers");
            return index >= other.index;
        }

        const_iterator& operator++() { ++index; return *this; }
        const_iterator operator++(int)
        {
            const_iterator ans = *this;
            ++index;
            return ans;
        }

        const_iterator& operator--() { --index; return *this; }
        const_iterator operator--(int)
        {
            const_iterator ans = *this;
            --index;
            return ans;
        }

        const_iterator& operator+=(int j) { index += j; return *this; }
        const_iterator& operator-=(int j) { index -= j; return *this; }
        const_iterator operator+(int j) const { return const_iterator(buffer, index + j); }
        const_iterator operator-(int j) const { return const_iterator(buffer, index - j); }
        int operator-(const_iterator other) const
        {
            Q_ASSERT_X(buffer == other.buffer, "QCircularBuffer<T>::const_iterator::operator-", "iterators use different buffers");
            return index - other.index;
        }

    private:
        const QCircularBuffer<T>* buffer;
        int index;
        friend class QCircularBuffer;
    };
    friend class const_iterator;

    iterator begin() { return iterator(this, 0); }
    const_iterator begin() const { return const_iterator(this, 0); }
    const_iterator constBegin() const { return const_iterator(this, 0); }
    iterator end() { return iterator(this, d->size); }
    const_iterator end() const { return const_iterator(this, d->size); }
    const_iterator constEnd() const { return const_iterator(this, d->size); }
    iterator insert(iterator before, int count, const T &value)
    {
        insert(before.index, count, value);
        return iterator(this, before.index);
    }
    iterator insert(iterator before, const T &value) { return insert(before, 1, value); }
    iterator erase(iterator begin, iterator end)
    {
        int count = end - begin;
        remove(begin.index, count);
        return iterator(this, end.index - count);
    }
    iterator erase(iterator pos) { return erase(pos, pos + 1); }

    // STL compatibility
    typedef T value_type;
    typedef value_type* pointer;
    typedef const value_type* const_pointer;
    typedef value_type& reference;
    typedef const value_type& const_reference;
    typedef ptrdiff_t difference_type;
    typedef iterator Iterator;
    typedef const_iterator ConstIterator;
    typedef int size_type;

    void push_back(const T &t) { append(t); }
    void push_front(const T &t) { prepend(t); }
    void pop_back() { Q_ASSERT(!isEmpty()); erase(end() - 1); }
    void pop_front() { Q_ASSERT(!isEmpty()); erase(begin()); }
    bool empty() const { return isEmpty(); }
    T &front() { return first(); }
    const T &front() const { return first(); }
    T &back() { return last(); }
    const T &back() const { return last(); }

    int refCount() const { return int(d->ref); }

    void append(const T &value);

    const T &at(int i) const
    {
        Q_ASSERT_X(i >= 0 && i < d->size, "QCircularBuffer<T>::at", "index out of range");
        int index = (d->first + i) % d->capacity;
        return d->data[ index ];
    }

    const T &operator[](int i) const
    {
        Q_ASSERT_X(i >= 0 && i < d->size, "QCircularBuffer<T>::operator[]", "index out of range");
        int index = (d->first + i) % d->capacity;
        return d->data[ index ];
    }

    T &operator[](int i)
    {
        Q_ASSERT_X(i >= 0 && i < d->size, "QCircularBuffer<T>::operator[]", "index out of range");
        int index = (d->first + i) % d->capacity;
        return d->data[ index ];
    }

    int capacity() const { return d->capacity; }

    void clear() { *this = QCircularBuffer<T>(d->capacity); }

    bool contains(const T &value) const;
    int count(const T &value) const;
    int count() const { return size(); }

    array_range data()
    {
        if (d->size == 0)
            return array_range(0, 0);
        if (!isLinearised())
            linearise();
        return array_range(d->data + d->first, d->last - d->first + 1);
    }
    const_array_range data() const { return constData(); }
    const_array_range constData() const
    {
        if (!isLinearised() || d->size == 0)
            return const_array_range(0, 0);
        return const_array_range(d->data + d->first, d->last - d->first + 1);
    }

    array_range dataOne()
    {
        if (d->size == 0)
            return array_range(0, 0);
        if (isLinearised())
            return array_range(d->data + d->first, d->last - d->first + 1);
        else
            return array_range(d->data + d->first, d->capacity - d->first);
    }
    const_array_range dataOne() const { return constDataOne(); }
    const_array_range constDataOne() const
    {
        if (d->size == 0)
            return const_array_range(0, 0);
        if (isLinearised())
            return const_array_range(d->data + d->first, d->last - d->first + 1);
        else
            return const_array_range(d->data + d->first, d->capacity - d->first);
    }

    array_range dataTwo()
    {
        if (d->size == 0 || isLinearised())
            return array_range(0, 0);
        return array_range(d->data, d->last + 1);
    }
    const_array_range dataTwo() const { return constDataTwo(); }
    const_array_range constDataTwo() const
    {
        if (d->size == 0 || isLinearised())
            return const_array_range(0, 0);
        return const_array_range(d->data, d->last + 1);
    }

    bool endsWith(const T &value) const { return !isEmpty() && last() == value; }
    QCircularBuffer<T>& fill(const T &value, int size = -1);
    T &first() { Q_ASSERT(!isEmpty()); return d->data[ d->first ]; }
    const T &first() const { Q_ASSERT(!isEmpty()); return d->data[ d->first ]; }
    int freeSize() const { return sizeAvailable(); }

    static QCircularBuffer<T> fromList(const QList<T>& list);
    static QCircularBuffer<T> fromVector(const QVector<T>& vector);

    int indexOf(const T &value, int from = 0) const;
    void insert(int i, const T &value) { insert(i, 1, value); };
    void insert(int i, int count, const T &value);
    bool isEmpty() const { return d->size == 0; }
    bool isFull() const { return d->size == d->capacity; }
    bool isLinearised() const { return (d->last >= d->first); }
    T &last() { Q_ASSERT(!isEmpty()); return d->data[ d->last ]; }
    const T &last() const { Q_ASSERT(!isEmpty()); return d->data[ d->last ]; }
    int lastIndexOf(const T &value, int from = -1) const;
    void linearise();
    void prepend(const T &value);
    void remove(int i) { remove(i, 1); }
    void remove(int i, int count);

    void replace(int i, const T &value)
    {
        Q_ASSERT_X(i >= 0 && i < d->size, "QCircularBuffer<T>::replace", "index out of range");
        const T copy(value);
        (*this)[ i ] = copy;
    }

    void reserve(int capacity) { setCapacity(capacity); }
    void resize(int size);
    void setCapacity(int capacity);
    int size() const { return d->size; }
    int sizeAvailable() const { return d->capacity - d->size; }
    void squeeze() { setCapacity(size()); }
    bool startsWith(const T &value) const { return !isEmpty() && first() == value; }

    QList<T> toList() const;
    QVector<T> toVector() const;

    T value(int i) const
    {
        if (i < 0 || i >= d->size)
            return T();
        return at(i);
    }

    T value(int i, const T &defaultValue) const
    {
        if (i < 0 || i >= d->size)
            return defaultValue;
        return at(i);
    }

    bool operator==(const QCircularBuffer<T> other) const;
    bool operator!=(const QCircularBuffer<T> other) const { return !(*this == other); }

    QCircularBuffer<T>& operator+=(const T &other) { append(other); return *this; }
    QCircularBuffer<T>& operator+=(const QCircularBuffer<T>& other);
    QCircularBuffer<T>& operator+=(const QVector<T>& other);
    QCircularBuffer<T>& operator+=(const QList<T>& other);

    QCircularBuffer<T>& operator<<(const T &other) { append(other); return *this; }
    QCircularBuffer<T>& operator<<(const QCircularBuffer<T>& other) { *this += other; return *this; }
    QCircularBuffer<T>& operator<<(const QVector<T>& other) { *this += other; return *this; }
    QCircularBuffer<T>& operator<<(const QList<T>& other) { *this += other; return *this; }

private:
    QSharedDataPointer< CircularBufferData<T> > d;
};

template <typename T>
QCircularBuffer<T> operator+(const QCircularBuffer<T>& lhs, const QCircularBuffer<T>& rhs);


template <typename T>
QCircularBuffer<T>::QCircularBuffer(int capacity)
    : d(new CircularBufferData<T>())
{
    // Allocate some raw memory
    d->data = d->allocate(capacity);
    d->capacity = capacity;

    // Initialise the objects. If the type T is a complex type then we do this
    // using the placement new operator. If the type T is not complex we simply
    // use memset to initialise the memory to zero.
    if (QTypeInfo<T>::isComplex) {
        T* b = d->data;
        T* i = b + d->capacity;
        while (i != b)
            new (--i) T;
    } else {
        qMemSet(d->data, 0, capacity * sizeof(T));
    }
}

template <typename T>
QCircularBuffer<T>::QCircularBuffer(int capacity, const T &value)
    : d(new CircularBufferData<T>())
{
    // Allocate some raw memory
    d->data = d->allocate(capacity);
    d->capacity = capacity;

    // Initialise the objects. In this case we always use the placement new operator
    T* b = d->data;
    T* i = b + d->capacity;
    while (i != b)
        new (--i) T(value);
    d->first = 0;
    d->last = d->capacity - 1;
    d->size = d->capacity;
}

template <typename T>
QCircularBuffer<T>::QCircularBuffer(int capacity, int size, const T &value)
    : d(new CircularBufferData<T>())
{
    Q_ASSERT_X(capacity >= size, "QCircularBuffer<T>::QCircularBuffer(int capacity, int size, const T &value)", "size is greater than capacity");

    // Allocate some raw memory
    d->data = d->allocate(capacity);
    d->capacity = capacity;

    // Initialise the objects that need to be set to the specified value.
    // In this case we always use the placement new operator
    T* b = d->data;
    T* i = b + size;
    while (i != b)
        new (--i) T(value);

    // Initialise the remaining objects using the best method available
    if (QTypeInfo<T>::isComplex) {
        T* b = d->data + size;
        T* i = b + capacity - size;
        while (i != b)
            new (--i) T;
    } else {
        qMemSet(d->data + size, 0, (capacity - size) * sizeof(T));
    }

    d->first = 0;
    d->last = size - 1;
    d->size = size;
}

template <typename T>
void QCircularBuffer<T>::append(const T &value)
{
    // If we have no capacity we do nothing
    if (!d->capacity)
        return;

    if (d->size == d->capacity) {
        // Buffer is full. Overwrite earliest item and rotate
        d->data[ d->first ] = value;
        d->first = (++d->first % d->capacity);
        d->last = (++d->last % d->capacity);
    } else if (d->size != 0) {
        // Buffer is partially full. Append data to end of array using appropriate method
        int index = (d->first + d->size) % d->capacity;
        if (QTypeInfo<T>::isComplex)
            new (d->data + index) T(value);
        else
            d->data[ index ] = value;
        ++d->size;
        ++d->last;
    } else {
        // Buffer is empty. Append data to end of array using appropriate method
        d->size = 1;
        d->first = d->last = 0;
        if (QTypeInfo<T>::isComplex)
            new (d->data + d->first) T(value);
        else
            d->data[ d->first ] = value;
    }
}

template <typename T>
bool QCircularBuffer<T>::contains(const T &value) const
{
    if (isLinearised()) {
        T* b = d->data + d->first;
        T* i = b + d->size;
        while (i != b)
            if (*--i == value)
                return true;
        return false;
    } else {
        // Check the array from m_first to the end
        T* b = d->data + d->first;
        T* i = d->data + d->capacity;
        while (i != b)
            if (*--i == value)
                return true;

        // Check array from 0 to m_end
        b = d->data;
        i = d->data + d->last + 1;
        while (i != b)
            if (*--i == value)
                return true;

        return false;
    }
}

template <typename T>
int QCircularBuffer<T>::count(const T &value) const
{
    int c = 0;
    if (isLinearised()) {
        T* b = d->data + d->first;
        T* i = b + d->size;
        while (i != b)
            if (*--i == value)
                ++c;
    } else {
        // Check the array from m_first to the end
        T* b = d->data + d->first;
        T* i = d->data + d->capacity;
        while (i != b)
            if (*--i == value)
                ++c;

        // Check array from 0 to m_end
        b = d->data;
        i = d->data + d->last + 1;
        while (i != b)
            if (*--i == value)
                ++c;
    }
    return c;
}

template <typename T>
QCircularBuffer<T>& QCircularBuffer<T>::fill(const T &value, int size)
{
    Q_ASSERT_X(d->capacity >= size, "QCircularBuffer<T>::fill", "size is greater than capacity");
    const T copy(value);
    int oldSize = d->size;
    d->size = (size < 0 ? d->size : size);
    d->first = (size == 0 ? -1 : 0);
    d->last = d->size - 1;

    // Copy item into array size times
    if (d->size) {
        T *b = d->data;
        T *i = d->data + d->size;
        while (i != b)
            *--i = copy;
    }

    if (d->size < oldSize) {
        // Cleanup old items beyond end of new array
        T *b = d->data + d->size;
        T *i = d->data + oldSize;
        while (i-- != b) {
            i->~T();
            new (i) T();
        }
    }

    return *this;
}

template <typename T>
QCircularBuffer<T> QCircularBuffer<T>::fromList(const QList<T>& list)
{
    QCircularBuffer<T> circ(list.size());
    for (int i = 0; i < list.size(); ++i)
        circ.append(list.at(i));
    return circ;
}

template <typename T>
QCircularBuffer<T> QCircularBuffer<T>::fromVector(const QVector<T>& vector)
{
    QCircularBuffer<T> circ(vector.size());
    for (int i = 0; i < vector.size(); ++i)
        circ.append(vector.at(i));
    return circ;
}

template <typename T>
int QCircularBuffer<T>::indexOf(const T &value, int from) const
{
    Q_ASSERT_X(from < d->size, "QCircularBuffer<T>::indexOf", "from is greater than last valid index");
    if (from < 0)
        from = qMax(from + d->size, 0);
    else if (from >= d->size)
        from = d->size - 1;
    for (int i = from; i < d->size; ++i)
        if (at(i) == value)
            return i;
    return -1;
}

template <typename T>
void QCircularBuffer<T>::insert(int i, int count, const T &value)
{
    Q_ASSERT_X(i >= 0 && i <= d->size, "QCircularBuffer<T>::insert", "index out of range");

    int freeCapacity = d->capacity - d->size;

    // Calculate number of elements that will actually be inserted. This
    // depends upon where the insertion has been requested and any spare
    // capacity left in the buffer. This is because elements at higher
    // indices will be pushed to the right and will potentially wrap around
    // to overwrite earlier elements.
    int numToInsert = qMin(count, i + freeCapacity);

    // Calculate the number of elements at the beginning of the buffer that
    // will be overwritten
    int numToOverwrite = qMin(i, qMax(0, count - freeCapacity));

    // Decide which way to shift to minimise the amount of copying required.
    if (i < d->size / 2) {
        // Inserting in lower half of buffer so we shift earlier items down

        // Shift data at the bottom end down. This may only be a subset if some
        // of the early data is to be overwritten.
        if (QTypeInfo<T>::isStatic) {
            int start = d->first + numToOverwrite;
            int end = d->first + i - 1;
            for (int j = start; j <= end; ++j) {
                int srcIndex = j % d->capacity;
                int dstIndex = (j - numToInsert + d->capacity) % d->capacity;
                T* src = d->data + srcIndex;
                T* dst = d->data + dstIndex;

                new (dst) T(*src);
            }
        } else {
            // We have a movable type so a simple memcopy (or maybe two or
            // three) will suffice to shift the data at the bottom end
            int numToMove = i - numToOverwrite;
            if (numToMove > 0) {
                int srcBegin = (d->first + numToOverwrite) % d->capacity;
                int srcEnd = (d->first + i - 1) % d->capacity;
                int dstBegin = (srcBegin - numToInsert + d->capacity) % d->capacity;
                int dstEnd = (srcEnd - numToInsert + d->capacity) % d->capacity;

                // Do we have any wrap-around problems to deal with?
                bool srcRegionWraps = (srcEnd < srcBegin);
                bool dstRegionWraps = (dstEnd < dstBegin);
                if (!srcRegionWraps && dstRegionWraps) {
                    // Destination region wraps so do the move in two steps
                    int wrapCount = abs(srcBegin - numToInsert);
                    memmove(d->data + d->capacity - wrapCount, d->data + srcBegin, wrapCount * sizeof(T));
                    memmove(d->data, d->data + srcBegin + wrapCount, (numToMove - wrapCount) * sizeof(T));
                } else if (srcRegionWraps && !dstRegionWraps) {
                    // Source region wraps so do the move in two steps
                    int wrapCount = d->capacity - srcBegin;
                    memmove(d->data + dstBegin, d->data + d->capacity - wrapCount, wrapCount * sizeof(T));
                    memmove(d->data + dstBegin + numToInsert, d->data, (numToMove - wrapCount) * sizeof(T));
                } else if (srcRegionWraps && dstRegionWraps) {
                    // Source and destination regions wrap so we have to do this in three steps
                    int srcWrapCount = d->capacity - srcBegin;
                    memmove(d->data + dstBegin, d->data + d->capacity - srcWrapCount, srcWrapCount * sizeof(T));
                    memmove(d->data + d->capacity - numToInsert, d->data, numToInsert * sizeof(T));
                    memmove(d->data, d->data + numToInsert, (numToMove - srcWrapCount - numToInsert) * sizeof(T));
                } else {
                    // No wrap around - do a single memmove
                    memmove(d->data + dstBegin, d->data + srcBegin, numToMove * sizeof(T));
                }
            }
        }

        // Insert the new items
        int end = d->first + i;
        int start = end - numToInsert;
        for (int j = start; j < end; ++j) {
            T* p = d->data + ((j + d->capacity) % d->capacity);
            new (p) T(value);
        }

        // Adjust the first, last and size indices as needed.
        // NB. The last index never changes in this regime.
        d->size += qMin(count, freeCapacity);
        d->first = (d->first - (numToInsert - numToOverwrite) + d->capacity) % d->capacity;
    } else {
        // Inserting in upper half of buffer so we shift later items up

        // Shift data at the top end up which may or may not overwrite some
        // of the earliest data
        if (QTypeInfo<T>::isStatic) {
            int start = d->first + d->size - 1;
            int end = d->first + i;
            for (int j = start; j >= end; j--) {
                int srcIndex = j % d->capacity;
                int dstIndex = (j + numToInsert) % d->capacity;
                T* src = d->data + srcIndex;
                T* dst = d->data + dstIndex;

                new (dst) T(*src);
            }
        } else {
            // We have a movable type so a simple memcopy (or maybe two or
            // three) will suffice to shift the data at the top end
            int numToMove = d->size - i;
            if (numToMove > 0) {
                int srcBegin = (d->first + i) % d->capacity;
                int srcEnd = d->last;
                int dstBegin = (srcBegin + numToInsert) % d->capacity;
                int dstEnd = (srcEnd + numToInsert) % d->capacity;

                // Do we have any wrap-around problems to deal with?
                bool srcRegionWraps = (srcEnd < srcBegin);
                bool dstRegionWraps = (dstEnd < dstBegin);
                if (!srcRegionWraps && dstRegionWraps) {
                    // Destination region wraps so do the move in two steps
                    int wrapCount = srcEnd + numToInsert - d->capacity + 1;
                    memmove(d->data, d->data + srcEnd - wrapCount + 1, wrapCount * sizeof(T));
                    memmove(d->data + dstBegin, d->data + srcBegin, (numToMove - wrapCount) * sizeof(T));
                } else if (srcRegionWraps && !dstRegionWraps) {
                    // Source region wraps so do the move in two steps
                    int wrapCount = d->last + 1;
                    memmove(d->data + numToInsert, d->data, wrapCount * sizeof(T));
                    memmove(d->data + dstBegin, d->data + srcBegin, (numToMove - wrapCount) * sizeof(T));
                } else if (srcRegionWraps && dstRegionWraps) {
                    // Source and destination regions wrap so we have to do this in three steps
                    int srcWrapCount = d->last + 1;
                    memmove(d->data + numToInsert, d->data, srcWrapCount * sizeof(T));
                    memmove(d->data, d->data + d->capacity - numToInsert, numToInsert * sizeof(T));
                    memmove(d->data + dstBegin, d->data + srcBegin, (numToMove - srcWrapCount - numToInsert) * sizeof(T));
                } else {
                    // No wrap around - do a single memmove
                    memmove(d->data + dstBegin, d->data + srcBegin, numToMove * sizeof(T));
                }
            }
        }

        // Insert the new items
        for (int j = d->first + i; j < d->first + i + numToInsert; ++j) {
            T* p = d->data + (j % d->capacity);
            new (p) T(value);
        }

        // Adjust the first, last and size indices as needed
        d->size += qMin(count, freeCapacity);
        d->first = (d->first + numToOverwrite) % d->capacity;
        d->last = (d->last + numToInsert) % d->capacity;
    }
}

template <typename T>
int QCircularBuffer<T>::lastIndexOf(const T &value, int from) const
{
    if (from < 0)
        from = qMax(from + d->size, 0);
    else if (from >= d->size)
        from = d->size - 1;
    for (int i = from; i >= 0; --i)
        if (at(i) == value)
            return i;
    return -1;
}

template <typename T>
void QCircularBuffer<T>::linearise()
{
    if (isLinearised())
        return;

    // Allocate some new raw memory
    T* newData = d->allocate(d->capacity);

    // Copy across the elements from the original array...
    // Copy items from m_first to the end of the array
    T* b = d->data + d->first;
    T* i = d->data + d->capacity;
    T* j = newData + d->capacity - d->first;
    while (i != b)
        new (--j) T(*--i);

    // Copy items from the start of the array to m_last
    b = d->data;
    i = d->data + d->last + 1;
    j = newData + d->size;
    while (i != b)
        new (--j) T(*--i);

    // Destroy the original items
    if (QTypeInfo<T>::isComplex) {
        // The type is complex so we manually call the destructor for each item
        // since we used the placement new operator to instantiate them
        T* b = d->data;
        T* i = b + d->capacity;
        while (i-- != b)
             i->~T();
    }

    // Release the raw memory
    d->deallocate(d->data);

    // Assign the new memory to be our buffer data and fix indices
    d->data = newData;
    d->first = 0;
    d->last = d->size - 1;
}

template <typename T>
void QCircularBuffer<T>::prepend(const T &value)
{
    // If we have no capacity we do nothing
    if (!d->capacity)
        return;

    if (d->size == d->capacity) {
        // Buffer is full. Overwrite last item and rotate
        d->data[ d->last ] = value;
        d->first = (--d->first + d->capacity) % d->capacity;
        d->last = (--d->last + d->capacity) % d->capacity;
    } else if (d->size != 0) {
        // Buffer is partially full. Prepend data to start of array using appropriate method
        d->first = (--d->first + d->capacity) % d->capacity;
        ++d->size;
        if (QTypeInfo<T>::isComplex)
            new (d->data + d->first) T(value);
        else
            d->data[ d->first ] = value;
    } else {
        // Buffer is empty. Prepend data to start of array using appropriate method
        d->size = 1;
        d->first = d->last = d->capacity - 1;
        if (QTypeInfo<T>::isComplex)
            new (d->data + d->first) T(value);
        else
            d->data[ d->first ] = value;
    }
}

template <typename T>
void QCircularBuffer<T>::remove(int i, int count)
{
    Q_ASSERT_X(i >= 0 && count > 0 && i + count <= d->size, "QCircularBuffer<T>::remove", "index out of range");

    // Calculate the number of items that need to be moved downward
    int numToMoveDown = d->size - count - i;
    int numToMoveUp = i;

    if (numToMoveDown < numToMoveUp) {
        // Move higher items down
        int numToMove = numToMoveDown;

        if (QTypeInfo<T>::isComplex) {
            // Copy items down from higher positions
            int start = d->first + i;
            int end = start + numToMove;
            for (int j = start; j < end ; ++j) {
                T* src = d->data + ((j + count) % d->capacity);
                T* dst = d->data + (j % d->capacity);
                new (dst) T(*src);
            }

            // Clean up items at end of buffer
            for (int j = d->last; j > d->last - count; --j) {
                T* p = d->data + ((j + d->capacity) % d->capacity);
                p->~T();
                new (p) T();
            }
        } else {
            if (isLinearised()) {
                // With a linearised buffer we can do a simple move and removal of items
                memmove(d->data + d->last - numToMove - count + 1, d->data + d->last - numToMove + 1, numToMove * sizeof(T));
                qMemSet(d->data + d->last - count + 1, 0, count * sizeof(T));
            } else {
                // With a non-linearised buffer we need to be careful of wrapping issues
                int srcBegin = (d->last - numToMove + 1 + d->capacity) % d->capacity;
                int srcEnd = d->last;
                int dstBegin = (d->first + i) % d->capacity;
                int dstEnd = (dstBegin + numToMove - 1) % d->capacity;

                bool srcRegionWraps = (srcEnd < srcBegin);
                bool dstRegionWraps = (dstEnd < dstBegin);
                if (srcRegionWraps && !dstRegionWraps) {
                    // Source region wraps so do the move in two steps
                    int wrapCount = d->capacity - srcBegin;
                    memmove(d->data + dstBegin, d->data + srcBegin, wrapCount * sizeof(T));
                    memmove(d->data + dstBegin + wrapCount, d->data, (numToMove - wrapCount) * sizeof(T));
                } else if (!srcRegionWraps && dstRegionWraps) {
                    // Destination region wraps so do the move in two steps
                    int wrapCount = count - srcBegin;
                    memmove(d->data + d->capacity - wrapCount, d->data + srcBegin, wrapCount * sizeof(T));
                    memmove(d->data, d->data + srcBegin + wrapCount, (numToMove - wrapCount) * sizeof(T));
                } else if (srcRegionWraps && dstRegionWraps) {
                    // Source and destination regions wrap so we have to do this in three steps
                    int srcWrapCount = d->capacity - srcBegin;
                    memmove(d->data + dstBegin, d->data + srcBegin, srcWrapCount * sizeof(T));
                    memmove(d->data + dstBegin + srcWrapCount, d->data, count * sizeof(T));
                    memmove(d->data, d->data + count, (numToMove - srcWrapCount - count) * sizeof(T));
                } else {
                    // No wrap around, so we can do this in one hit
                    memmove(d->data + dstBegin, d->data + srcBegin, numToMove * sizeof(T));
                }

                // We potentially have a disjoint region that needs zeroing
                int zeroStart = (d->last - count + d->capacity + 1) % d->capacity;
                int zeroEnd = d->last;
                if (zeroEnd < zeroStart) {
                    // Region to be zeroed wraps. Do it in two steps.
                    qMemSet(d->data, 0, (d->last + 1) * sizeof(T));
                    qMemSet(d->data + zeroStart, 0, (count - d->last - 1) * sizeof(T));
                } else {
                    // Region to be zeroed is contiguous
                    qMemSet(d->data + zeroStart, 0, count * sizeof(T));
                }
            }
        }

        // Adjust the indices
        d->size -= count;
        d->last = (d->last - count + d->capacity) % d->capacity;
    } else {
        // Move lower items up
        int numToMove = numToMoveUp;

        if (QTypeInfo<T>::isComplex) {
            // Copy items up from lower positions
            int start = d->first + i - 1;
            int end = start - numToMove;
            for (int j = start; j > end ; --j) {
                T* src = d->data + ((j + d->capacity) % d->capacity);
                T* dst = d->data + ((j + d->capacity + count) % d->capacity);
                new (dst) T(*src);
            }

            // Clean up items at start of buffer
            for (int j = d->first; j < d->first + count; ++j) {
                T* p = d->data + (j % d->capacity);
                p->~T();
                new (p) T();
            }
        } else {
            if (isLinearised()) {
                // With a linearised buffer we can do a simple move and removal of items
                memmove(d->data + d->first + count, d->data + d->first, numToMove * sizeof(T));
                qMemSet(d->data + d->first, 0, count * sizeof(T));
            } else {
                // With a non-linearised buffer we need to be careful of wrapping issues
                int srcBegin = d->first;
                int srcEnd = (srcBegin + numToMove - 1) % d->capacity;
                int dstBegin = (srcBegin + count) % d->capacity;
                int dstEnd = (dstBegin + numToMove - 1) % d->capacity;

                bool srcRegionWraps = (srcEnd < srcBegin);
                bool dstRegionWraps = (dstEnd < dstBegin);
                if (srcRegionWraps && !dstRegionWraps) {
                    // Source region wraps so do the move in two steps
                    int wrapCount = srcEnd + 1;
                    memmove(d->data + dstEnd - wrapCount + 1, d->data, wrapCount * sizeof(T));
                    memmove(d->data + dstBegin, d->data + srcBegin, (numToMove - wrapCount) * sizeof(T));
                } else if (!srcRegionWraps && dstRegionWraps) {
                    // Destination region wraps so do the move in two steps
                    int wrapCount = dstEnd + 1;
                    memmove(d->data, d->data + srcEnd - wrapCount + 1, wrapCount * sizeof(T));
                    memmove(d->data + dstBegin, d->data + srcBegin, (numToMove - wrapCount) * sizeof(T));
                } else if (srcRegionWraps && dstRegionWraps) {
                    // Source and destination regions wrap so we have to do this in three steps
                    int srcWrapCount = srcEnd + 1;
                    memmove(d->data + dstEnd - srcWrapCount + 1, d->data, srcWrapCount * sizeof(T));
                    memmove(d->data, d->data + d->capacity - count, count * sizeof(T));
                    memmove(d->data + dstBegin, d->data + srcBegin, (numToMove - srcWrapCount - count) * sizeof(T));
                } else {
                    // No wrap around, so we can do this in one hit
                    memmove(d->data + dstBegin, d->data + srcBegin, numToMove * sizeof(T));
                }

                // We potentially have a disjoint region that needs zeroing
                int zeroStart = d->first;
                int zeroEnd = (zeroStart + count - 1) % d->capacity;
                if (zeroEnd < zeroStart) {
                    // Region to be zeroed wraps. Do it in two steps.
                    qMemSet(d->data + zeroStart, 0, (d->capacity - d->first) * sizeof(T));
                    qMemSet(d->data, 0, (count - d->capacity + d->first) * sizeof(T));
                } else {
                    // Region to be zeroed is contiguous
                    qMemSet(d->data + zeroStart, 0, count * sizeof(T));
                }
            }
        }

        // Adjust the indices
        d->size -= count;
        d->first = (d->first + count) % d->capacity;
    }
}

template <typename T>
void QCircularBuffer<T>::setCapacity(int capacity)
{
    if (capacity == d->capacity)
        return;

    // Allocate some new raw memory
    T* newData = d->allocate(capacity);

    // How many items can we copy across?
    int newSize = qMin(d->size, capacity);

    if (QTypeInfo<T>::isComplex) {
        // Copy across the elements from the original array
        for (int i = 0; i < newSize; ++i) {
            T* src = d->data + ((d->first + i) % d->capacity);
            T* dst = newData + i;
            new (dst) T(*src);
        }

        // Initialise any memory outside of the valid buffer (ie the unused items)
        for (int i = newSize; i < capacity; ++i)
            new (newData + i) T();

        // Destroy the original items.
        // The type is complex so we manually call the destructor for each item
        // since we used the placement new operator to instantiate them
        T* b = d->data;
        T* i = b + d->capacity;
        while (i-- != b)
             i->~T();
    } else {
        // Copy across the elements from the original array. The source region
        // potentially wraps so we may have to do this in one or two steps
        if (isLinearised()) {
            memmove(newData, d->data + d->first, newSize * sizeof(T));
        } else {
            int step1Size = qMin(newSize, d->capacity - d->first);
            memmove(newData, d->data + d->first, step1Size * sizeof(T));
            int step2Size = qMax(0, qMin(newSize - d->capacity + d->first, d->last + 1));
            memmove(newData + step1Size, d->data, step2Size * sizeof(T));
        }

        // Initialise any memory outside of the valid buffer (ie the unused items)
        qMemSet(newData + newSize, 0, (capacity - newSize) * sizeof(T));
    }

    // Release the raw memory for the old array
    d->deallocate(d->data);

    // Assign the new memory to be our buffer data and fix indices
    d->data = newData;
    d->capacity = capacity;
    d->first = 0;
    d->size = newSize;
    d->last = d->size - 1;
}

template <typename T>
void QCircularBuffer<T>::resize(int size)
{
    Q_ASSERT_X(size >= 0 && size <= d->capacity, "QCircularBuffer<T>::resize", "size out of range");

    if (size < d->size) {
        remove(size, d->size - size);
    } else if (size > d->size) {
        T t;
        insert(d->size, size - d->size, t);
    }
}

template <typename T>
bool QCircularBuffer<T>::operator==(const QCircularBuffer<T> other) const
{
    if (capacity() != other.capacity() || size() != other.size())
        return false;
    if (d == other.d)
        return true;
    int count = size();
    for (int i = 0; i < count; ++i)
        if (at(i) != other.at(i))
            return false;
    return true;
}

template <typename T>
QCircularBuffer<T>& QCircularBuffer<T>::operator+=(const QCircularBuffer<T>& other)
{
    // How many items do we need to copy? No point in ever copying across a number
    // greater than capacity
    int numToCopy = qMin(other.size(), d->capacity);
    int offset = other.size() - numToCopy;
    for (int i = 0; i < numToCopy; ++i)
        append(other.at(offset + i));
    return *this;
}

template <typename T>
QCircularBuffer<T>& QCircularBuffer<T>::operator+=(const QVector<T>& other)
{
    // How many items do we need to copy? No point in ever copying across a number
    // greater than capacity
    int numToCopy = qMin(other.size(), d->capacity);
    int offset = other.size() - numToCopy;
    for (int i = 0; i < numToCopy; ++i)
        append(other.at(offset + i));
    return *this;
}

template <typename T>
QCircularBuffer<T>& QCircularBuffer<T>::operator+=(const QList<T>& other)
{
    // How many items do we need to copy? No point in ever copying across a number
    // greater than capacity
    int numToCopy = qMin(other.size(), d->capacity);
    int offset = other.size() - numToCopy;
    for (int i = 0; i < numToCopy; ++i)
        append(other.at(offset + i));
    return *this;
}

template <typename T>
QList<T> QCircularBuffer<T>::toList() const
{
    QList<T> list;
    for (int i = 0; i < size(); ++i)
        list.append(at(i));
    return list;
}

template <typename T>
QVector<T> QCircularBuffer<T>::toVector() const
{
    QVector<T> vector;
    for (int i = 0; i < size(); ++i)
        vector.append(at(i));
    return vector;
}

template <typename T>
QCircularBuffer<T> operator+(const QCircularBuffer<T>& lhs, const QCircularBuffer<T>& rhs)
{
    QCircularBuffer<int> circ(lhs.size() + rhs.size());
    for (int i = 0; i < lhs.size(); ++i)
        circ.append(lhs.at(i));
    for (int i = 0; i < rhs.size(); ++i)
        circ.append(rhs.at(i));
    return circ;
}

Q_DECLARE_SEQUENTIAL_ITERATOR(CircularBuffer)
Q_DECLARE_MUTABLE_SEQUENTIAL_ITERATOR(CircularBuffer)

QT_END_NAMESPACE

QT_END_HEADER

#endif // QCIRCULARBUFFER_H
