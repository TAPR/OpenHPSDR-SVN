#ifndef ABOUT_H
#define ABOUT_H

#include <QWidget>
#include <QMessageBox>

class About : public QWidget
{
    Q_OBJECT
public:
    explicit About(QWidget *parent = 0);
    QString version;

signals:

public slots:
    void aboutMessage();

};

#endif // ABOUT_H
