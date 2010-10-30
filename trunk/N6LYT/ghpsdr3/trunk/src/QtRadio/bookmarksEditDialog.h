#ifndef BOOKMARKSEDITDIALOG_H
#define BOOKMARKSEDITDIALOG_H

#include <QDialog>

namespace Ui {
    class bookmarksEditDialog;
}

class bookmarksEditDialog : public QDialog
{
    Q_OBJECT

public:
    explicit bookmarksEditDialog(QWidget *parent = 0);
    ~bookmarksEditDialog();

private:
    Ui::bookmarksEditDialog *ui;
};

#endif // BOOKMARKSEDITDIALOG_H
