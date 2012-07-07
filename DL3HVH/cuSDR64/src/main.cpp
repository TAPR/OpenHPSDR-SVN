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

#include "cusdr_splash.h"
#include "cusdr_settings.h"
#include "cusdr_mainWidget.h"

#include <QtGui>
#include <QApplication>
#include <QMessageBox>
#include <QDebug>
#include <QTime>
#include <QTextBrowser>
#include <QThread>
#include <QtOpenGL/QGLFramebufferObject>

#ifndef QT_NO_CONCURRENT


void cuSDRMessageHandler(QtMsgType type, const char *msg) {

    QString txt;
    QDateTime date;

    txt = msg;
    txt.prepend(": ");
    txt.prepend(date.currentDateTime().toString());

    QFile outFile("cuSDR.log");
    outFile.open(QIODevice::WriteOnly | QIODevice::Append);
    QTextStream ts(&outFile);
    ts << txt << endl << flush;
}

int main(int argc, char *argv[]) {

	#if (QT_VERSION < 0x040800)
		#error("You need Qt v4.8.0 or later to compile this");
	#endif

	//qInstallMsgHandler(cuSDRMessageHandler);
    QApplication app(argc, argv);

    Settings::instance(&app);

    app.setApplicationName(Settings::instance()->titleStr());
    app.setApplicationVersion(Settings::instance()->versionStr());

	class SleeperThread : public QThread {
	
	public:
		static void msleep(unsigned long msecs) {QThread::msleep(msecs);}
	};

    QPixmap splash_pixmap(":/img/cusdrLogo2.png");
	
	CSplashScreen* splash = new CSplashScreen(splash_pixmap);
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
		Settings::instance()->titleStr() + " " +
		Settings::instance()->versionStr() + 
		QObject::tr(":   Loading settings .."),
        Qt::AlignTop | Qt::AlignLeft, Qt::yellow);
	SleeperThread::msleep(1000);
    
    Settings::instance()->setSettingsFilename(QCoreApplication::applicationDirPath() +
        "/" + Settings::instance()->getSettingsFilename());

    Settings::instance()->setSettingsLoaded(Settings::instance()->loadSettings() >= 0);

    if (Settings::instance()->settingsLoaded()) {

        splash->showMessage(
			"\n      " + 
			Settings::instance()->titleStr() + " " +
			Settings::instance()->versionStr() + 
			QObject::tr(":   Settings loaded."),
			Qt::AlignTop | Qt::AlignLeft, Qt::yellow);
    }
    else {

        splash->showMessage(
			"\n      " + 
			Settings::instance()->titleStr() + " " +
			Settings::instance()->versionStr() + 
			QObject::tr(":   Settings not loaded."),
			Qt::AlignTop | Qt::AlignLeft, Qt::red);
    }
	SleeperThread::msleep(100);
    
	// ****************************
	// check for OpenGL

	splash->showMessage(
			"\n      " + 
			Settings::instance()->titleStr() + " " +
			Settings::instance()->versionStr() + 
			QObject::tr(":   Checking for OpenGL V 2.0 ..."),
			Qt::AlignTop | Qt::AlignLeft, Qt::yellow);
	SleeperThread::msleep(100);

	if (!QGLFormat::hasOpenGL() && QGLFormat::OpenGL_Version_2_0) {

		qDebug() << "Init::\t OpenGL not found!";
		splash->showMessage(
			"\n      " + 
			Settings::instance()->titleStr() + " " +
			Settings::instance()->versionStr() + 
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

		qDebug() << "Init::\t OpenGL found, but appears to be less than OGL v2.0.";
		splash->showMessage(
			"\n      " + 
			Settings::instance()->titleStr() + " " +
			Settings::instance()->versionStr() + 
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

	qDebug() << "Init::\t OpenGL found.";
	splash->showMessage(
			"\n      " + 
			Settings::instance()->titleStr() + " " +
			Settings::instance()->versionStr() + 
			QObject::tr(":   OpenGL found."),
			Qt::AlignTop | Qt::AlignLeft, Qt::yellow);

	SleeperThread::msleep(100);

	if (!QGLFramebufferObject::hasOpenGLFramebufferObjects()) {

		qDebug() << "Init:: Framebuffer Objects not found!\n";
		splash->showMessage(
			"\n      " + 
			Settings::instance()->titleStr() + " " +
			Settings::instance()->versionStr() + 
			QObject::tr(":   fbuffers not found!"),
			Qt::AlignTop | Qt::AlignLeft, Qt::yellow);

		Settings::instance()->setFBOPresence(false);
		SleeperThread::msleep(100);
		return -1;
		//splash->hide();
	}
	else {

		qDebug() << "Init::\t Framebuffer Objects found.";
		splash->showMessage(
			"\n      " + 
			Settings::instance()->titleStr() + " " +
			Settings::instance()->versionStr() + 
			QObject::tr(":   OpenGL Frame Buffer support found."),
			Qt::AlignTop | Qt::AlignLeft, Qt::yellow);

		Settings::instance()->setFBOPresence(true);
	}
	SleeperThread::msleep(100);


	// ****************************
	// check for OpenCL devices
	/*QList<QCLDevice> clDevices = QCLDevice::allDevices();
	
	splash->showMessage(
			"\n      " + 
			Settings::instance()->titleStr() + " " +
			Settings::instance()->versionStr() + 
			QObject::tr(":   Checking for OpenCL devices..."),
			Qt::AlignTop | Qt::AlignLeft, Qt::yellow);
	SleeperThread::msleep(100);

	if (clDevices.length() == 0) {
	
		qDebug() << "Init:: no OpenCL devices found!\n";
		splash->showMessage(
			"\n      " + 
			Settings::instance()->titleStr() + " " +
			Settings::instance()->versionStr() + 
			QObject::tr(":   no OpenCL devices found!"),
			Qt::AlignTop | Qt::AlignLeft, Qt::red);

		SleeperThread::msleep(1000);
	}
	else {

		qDebug() << "Init:: found" << clDevices.length() << "OpenCL device(s).";
		QString clNo = QString::number(clDevices.length());
		splash->showMessage(
			"\n      " + 
			Settings::instance()->titleStr() + " " +
			Settings::instance()->versionStr() + 
			QObject::tr(":   found ") +	clNo +
			QObject::tr(" OpenCL device(s)."),
			Qt::AlignTop | Qt::AlignLeft, Qt::green);

		SleeperThread::msleep(200);
	}*/

	//for (int i = 0; i < clDevices.length(); i++)
	//	qDebug() << clDevices.at(i);


	//QCLContext context;
	////if (!context.create(QCLDevice::CPU)) {
	////if (!context.create()) {
	//if (!context.create(QCLDevice::GPU)) {

	//	qDebug() << "Could not create OpenCL context.";
	//  return 1;
	//}
	//else
	//	qDebug() << "OpenCL context for the GPU created.";

	//QCLVector<int> input1 = context.createVector<int>(2048);
	//QCLVector<int> input2 = context.createVector<int>(2048);
	//   
	//for (int index = 0; index < 2048; ++index) {

	//    input1[index] = index;
	//    input2[index] = 2048 - index;
	//}

	//QCLVector<int> output = context.createVector<int>(2048);

	//qDebug() << "build OpenCL program from source...";
	//QCLProgram program = context.buildProgramFromSourceFile(":/cl/vectoradd.cl");
	//qDebug() << "done.";
	//QCLKernel kernel = program.createKernel("vectorAdd");

	//kernel.setGlobalWorkSize(2048);
	//kernel(input1, input2, output);

	//for (int index = 0; index < 2048; ++index) {

	//   if (output[index] != 2048) {

	//        qDebug() << "Answer at index %1 is %2, should be %3." <<  index << output[index] << 2048;
	//        return 1;
	//   }
	//}

	//qDebug() << "Answer is correct:" << 2048;


	// ****************************
	// setup main window

	splash->showMessage(
			"\n      " + 
			Settings::instance()->titleStr() + " " +
			Settings::instance()->versionStr() + 
			QObject::tr(":   setting up main window .."),
			Qt::AlignTop | Qt::AlignLeft, Qt::yellow);

	qDebug() << "Init::\t main window setup ...";
	MainWindow mainWindow;
	mainWindow.setup();
	qDebug() << "Init::\t main window setup done.";

	splash->showMessage(
			"\n      " + 
			Settings::instance()->titleStr() + " " +
			Settings::instance()->versionStr() + 
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
	//splash->hide();
	delete splash;

	mainWindow.update();
	mainWindow.setFocus();

	qDebug() << "Init::\t running application ...\n";
	
    return app.exec();
}

#else

int main() {
	
	qDebug() << "Qt Concurrent is not supported on this platform";
}

#endif