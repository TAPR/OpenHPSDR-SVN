#include "preferences.h"
#include "ui_preferences.h"
#include <QDebug>

Preferences::Preferences(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::Preferences)
{
    ui->setupUi(this);

    QCoreApplication::setOrganizationName("openhpsdr");
    QCoreApplication::setOrganizationDomain("openhpsdr.org");
    QCoreApplication::setApplicationName("QtLogger");

    loadPreferences( "preferences_radio" );
    loadPreferences( "preferences_location" );
    loadPreferences( "preferences_event" );
    loadPreferences( "preferences_owner" );
    loadPreferences( "preferences_qsl" );

    connect(ui->cancelButton,SIGNAL(clicked()),this,SLOT(close()));
    connect(ui->okButton,SIGNAL(clicked()),this,SLOT(updatePreferences()));
}

Preferences::~Preferences()
{
    delete ui;
}

void Preferences::updatePreferences()
{
    settings.setValue("preference_radio", getPreferences("radio"));
    settings.setValue("preference_location", getPreferences("location"));
    settings.setValue("preference_event", getPreferences("event"));
    settings.setValue("preference_owner", getPreferences("owner"));
    settings.setValue("preference_qsl", getPreferences("qsl"));
    settings.setValue("callsign_filter",getPreferences("callfilter"));

    emit preferencesChanged();
    this->close();
}

bool Preferences::getPreferences( QString test )
{
    if( test == "radio" ){
        return( ui->radio_checkBox->checkState() );
    }else if( test == "location" ){
        return( ui->location_checkBox->checkState() );
    }else if( test == "event" ){
        return( ui->event_checkBox->checkState() );
    }else if( test == "owner" ){
        return( ui->owner_checkBox->checkState() );
    }else if( test == "qsl" ){
        return( ui->qsl_checkBox->checkState() );
    }else if( test == "callfilter" ){
        return( ui->callfilter_checkBox->checkState() );
    }
    return( false );
}

void Preferences::loadPreferences( QString type )
{
    ui->radio_checkBox->setChecked(settings.value("preference_radio").toBool());
    ui->location_checkBox->setChecked(settings.value("preference_location").toBool());
    ui->event_checkBox->setChecked(settings.value("preference_event").toBool());
    ui->owner_checkBox->setChecked(settings.value("preference_qwner").toBool());
    ui->qsl_checkBox->setChecked(settings.value("preference_qsl").toBool());
    ui->callfilter_checkBox->setChecked(settings.value("callsign_filter").toBool());
}

void Preferences::setPreferences( QString type, QString checked )
{

}
