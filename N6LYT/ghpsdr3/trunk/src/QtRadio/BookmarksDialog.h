#ifndef BOOKMARKSDIALOG_H
#define BOOKMARKSDIALOG_H

#include <QDialog>
#include <QVector>

#include "Bookmark.h"

namespace Ui {
    class BookmarksDialog;
}

class BookmarksDialog : public QDialog
{
    Q_OBJECT

public:
    explicit BookmarksDialog(QWidget *parent,QVector<Bookmark*> bookmarks);
    ~BookmarksDialog();

    int getSelected();

private:
    Ui::BookmarksDialog *ui;
};

#endif // BOOKMARKSDIALOG_H
