#ifndef BANDLIMIT_H
#define BANDLIMIT_H

class BandLimit
{
public:
    BandLimit();
    BandLimit(long long min,long long max);
    long long min();
    long long max();

private:
    long long min_frequency;
    long long max_frequency;

};

#endif // BANDLIMIT_H
