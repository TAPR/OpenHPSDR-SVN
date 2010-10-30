#include "bookmarksEditDialog.h"
#include "ui_bookmarksEditDialog.h"

bookmarksEditDialog::bookmarksEditDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::bookmarksEditDialog)
{
    ui->setupUi(this);
}

bookmarksEditDialog::~bookmarksEditDialog()
{
    delete ui;
}
