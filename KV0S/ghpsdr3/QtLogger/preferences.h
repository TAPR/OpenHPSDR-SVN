#ifndef PREFERENCES_H
#define PREFERENCES_H

#include <QDialog>
#include <QSettings>

namespace Ui {
class Preferences;
}

class Preferences : public QDialog
{
    Q_OBJECT
    
public:
    explicit Preferences(QWidget *parent = 0);
    bool getPreferences( QString test );
    void setPreferences(QString type, QString checked );
    void loadPreferences(QString type );
    ~Preferences();

signals:
    void preferencesChanged();

private:
    Ui::Preferences *ui;
    QSettings settings;

private slots:
    void updatePreferences();
};

#endif // PREFERENCES_H
