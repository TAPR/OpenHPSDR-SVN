/**
* @file  main.cpp
* @brief main
* @author Hermann von Hasseln, DL3HVH
* @version 0.1
* @date 2010-09-08
*/

/*   
 *   Copyright 2010, 2011, 2012 Hermann von Hasseln, DL3HVH
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU Library General Public License version 2 as
 *   published by the Free Software Foundation
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details
 *
 *   You should have received a copy of the GNU Library General Public
 *   License along with this program; if not, write to the
 *   Free Software Foundation, Inc.,
 *   51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

#include "Util/cusdr_splash.h"
#include "cusdr_settings.h"


#if defined(Q_OS_WIN32)
	#include "Util/cusdr_cpuUsage.h"
#elif defined(Q_OS_LINUX)
    #include "Util/cusdr_cpuUsage_unix.h"
#endif

#include "cusdr_mainWidget.h"
#include <cufftw.h>

//#include <QtGui>
#include <QApplication>
#include <QMessageBox>
#include <QDebug>
//#include <QTime>
#include <QTextBrowser>
#include <QThread>
//#include <QtOpenGL/QGLFramebufferObject>

//#ifndef _WIN32_WINNT            // Specifies that the minimum required platform is Windows Vista.
//#define _WIN32_WINNT 0x0600     // Change this to the appropriate value to target other versions of Windows.
//#endif

// CPU usage
#if defined(Q_OS_WIN32)
	DWORD WINAPI WatchItThreadProc(LPVOID lpParam);
	CpuUsage usage;

	DWORD WINAPI WatchItThreadProc(LPVOID lpParam) {

		Q_UNUSED(lpParam)

		DWORD dummy;
		while (true) {

			short cpuUsage = usage.GetUsage();
			Settings::instance()->setCPULoad(cpuUsage);

			Sleep(1000);
		}
		return dummy;
	}
#endif

void cuSDRMessageHandler(QtMsgType type, const QMessageLogContext &context, const QString &msg) {

	Q_UNUSED(type)

    QString txt;
    QDateTime date;

    txt = msg;
    txt.prepend(": ");
    txt.prepend(date.currentDateTime().toString());

    QFile outFile("cudaSDR.log");
    outFile.open(QIODevice::WriteOnly | QIODevice::Append);
    QTextStream ts(&outFile);
    ts << txt << endl << flush;
}

int main(int argc, char *argv[]) {

	#ifndef DEBUG
        qInstallMessageHandler(cuSDRMessageHandler);
	#endif

    QApplication app(argc, argv);

    Settings::instance(&app);

    app.setApplicationName(Settings::instance()->getTitleStr());
    app.setApplicationVersion(Settings::instance()->getVersionStr());

	class SleeperThread : public QThread {
	
	public:
		static void msleep(unsigned long msecs) {QThread::msleep(msecs);}
	};

    QPixmap splash_pixmap(":/img/cudaSDRLogo.png");

    CSplashScreen* splash = new CSplashScreen(splash_pixmap);

	splash->setGeometry(
				QStyle::alignedRect(
				Qt::LeftToRight,
				Qt::AlignCenter,
				splash->size(),
				app.desktop()->availableGeometry()));

    splash->show();

    float splash_transparency = 0;
    QTime splash_fade_timer;
    splash_fade_timer.start();

    while (splash_transparency < 1) {

        splash_transparency = (float)splash_fade_timer.elapsed() / 500;//1000;
        if (splash_transparency > 1) splash_transparency = 1;
        splash->setWindowOpacity(splash_transparency);
        splash->repaint();
    }

	splash->showMessage(
		"\n      " + 
		Settings::instance()->getTitleStr() + " " +
		Settings::instance()->getVersionStr() +
		QObject::tr(":   Loading settings .."),
        Qt::AlignTop | Qt::AlignLeft, Qt::yellow);

    SleeperThread::msleep(500);

    Settings::instance()->setSettingsFilename(QCoreApplication::applicationDirPath() +
        "/" + Settings::instance()->getSettingsFilename());

    Settings::instance()->setSettingsLoaded(Settings::instance()->loadSettings() >= 0);

    if (Settings::instance()->getSettingsLoaded()) {

        splash->showMessage(
			"\n      " + 
			Settings::instance()->getTitleStr() + " " +
			Settings::instance()->getVersionStr() +
			QObject::tr(":   Settings loaded."),
			Qt::AlignTop | Qt::AlignLeft, Qt::yellow);

        SleeperThread::msleep(100);
    }
    else {

        splash->showMessage(
			"\n      " + 
			Settings::instance()->getTitleStr() + " " +
			Settings::instance()->getVersionStr() +
			QObject::tr(":   Settings not loaded."),
			Qt::AlignTop | Qt::AlignLeft, Qt::red);

		//splash->showMessage("\n      " + QObject::tr(copyright), Qt::AlignBottom | Qt::AlignLeft, Qt::black);
    }

	// ****************************
	// check for OpenGL

	splash->showMessage(
			"\n      " + 
			Settings::instance()->getTitleStr() + " " +
			Settings::instance()->getVersionStr() +
			QObject::tr(":   Checking for OpenGL V 2.0 ..."),
			Qt::AlignTop | Qt::AlignLeft, Qt::yellow);

    SleeperThread::msleep(100);

	if (!QGLFormat::hasOpenGL() && QGLFormat::OpenGL_Version_2_0) {

		qDebug() << "Init::\tOpenGL not found!";
		splash->showMessage(
			"\n      " + 
			Settings::instance()->getTitleStr() + " " +
			Settings::instance()->getVersionStr() +
			QObject::tr(":   not found!"),
			Qt::AlignTop | Qt::AlignLeft, Qt::red);

		splash->hide();

		QMessageBox::critical(0, 
			QApplication::applicationName(), 
			QApplication::applicationName() + "   requires OpenGL v2.0 or later to run.", 
			QMessageBox::Abort);

		return -1;
	}

	if (!(QGLFormat::openGLVersionFlags() & QGLFormat::OpenGL_Version_2_0)) {

		qDebug() << "Init::\tOpenGL found, but appears to be less than OGL v2.0.";
		splash->showMessage(
			"\n      " + 
			Settings::instance()->getTitleStr() + " " +
			Settings::instance()->getVersionStr() +
			QObject::tr(":   found but appears to be less than OGL v2.0"),
			Qt::AlignTop | Qt::AlignLeft, Qt::yellow);

		splash->hide();

		QMessageBox::critical(0, 
			QApplication::applicationName(), 
			QApplication::applicationName() + "   requires OpenGL v2.0 or later to run.", 
			QMessageBox::Ok);

		return -1;
	}
	//if (QGLFormat::OpenGL_Version_2_0)
	//	qDebug() << "OpenGL version > 2.0";

	qDebug() << "Init::\tOpenGL found.";
	splash->showMessage(
			"\n      " + 
			Settings::instance()->getTitleStr() + " " +
			Settings::instance()->getVersionStr() +
			QObject::tr(":   OpenGL found."),
			Qt::AlignTop | Qt::AlignLeft, Qt::yellow);

    SleeperThread::msleep(100);

	if (!QGLFramebufferObject::hasOpenGLFramebufferObjects()) {

		qDebug() << "Init::Framebuffer Objects not found!\n";
		splash->showMessage(
			"\n      " + 
			Settings::instance()->getTitleStr() + " " +
			Settings::instance()->getVersionStr() +
			QObject::tr(":   fbuffers not found!"),
			Qt::AlignTop | Qt::AlignLeft, Qt::yellow);

		Settings::instance()->setFBOPresence(false);
        SleeperThread::msleep(100);
		return -1;
		//splash->hide();
	}
	else {

		qDebug() << "Init::\tFramebuffer Objects found.";
		splash->showMessage(
			"\n      " + 
			Settings::instance()->getTitleStr() + " " +
			Settings::instance()->getVersionStr() +
			QObject::tr(":   OpenGL Frame Buffer support found."),
			Qt::AlignTop | Qt::AlignLeft, Qt::yellow);

		Settings::instance()->setFBOPresence(true);
	}
    SleeperThread::msleep(100);

	// ****************************
	
	// cpu usage
#if defined(Q_OS_WIN32)
	CreateThread(NULL, 0, WatchItThreadProc, NULL, 0, NULL);
#endif

	// ****************************
	// setup main window

	splash->showMessage(
			"\n      " + 
			Settings::instance()->getTitleStr() + " " +
			Settings::instance()->getVersionStr() +
			QObject::tr(":   setting up main window .."),
			Qt::AlignTop | Qt::AlignLeft, Qt::yellow);

	qDebug() << "Init::\tmain window setup ...";
    MainWindow mainWindow;
    mainWindow.setup();
	qDebug() << "Init::\tmain window setup done.";

	splash->showMessage(
			"\n      " + 
			Settings::instance()->getTitleStr() + " " +
			Settings::instance()->getVersionStr() +
			QObject::tr(":   Displaying main window .."),
			Qt::AlignTop | Qt::AlignLeft, Qt::yellow);

    SleeperThread::msleep(300);
	
    //app.processEvents();

    mainWindow.show();
    app.processEvents();

	/*splash_fade_timer.restart();
	while (splash_transparency > 0)
	{
		splash_transparency = 1 - (float)splash_fade_timer.elapsed() / 300;
		if (splash_transparency < 0) splash_transparency = 0;
		if (splash_transparency > 1) splash_transparency = 1;
		splash->setWindowOpacity(splash_transparency);
		splash->repaint();
	}*/
    splash->hide();

	delete splash;

	mainWindow.update();
	mainWindow.setFocus();

#if defined(Q_OS_LINUX)
    cusdr_cpuUsage *cpu_load = new cusdr_cpuUsage();
    cpu_load->start();
#endif

	qDebug() << "Init::\trunning application ...\n";

    return app.exec();
}
