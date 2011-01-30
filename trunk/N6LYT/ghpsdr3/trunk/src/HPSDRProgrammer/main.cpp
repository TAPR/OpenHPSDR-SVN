#include <QtGui/QApplication>
#include "mainwindow.h"

//  This is for the Mac permissions elevation
//  Jeremy McDermond (NH6Z)
#ifdef __APPLE__ && __MACH__
#include <sys/param.h>
#include <mach-o/dyld.h>
#include <Authorization.h>
#include <AuthorizationTags.h>
#endif // __APPLE__ && __MACH__



int main(int argc, char *argv[])
{
        //  Gain admin privileges on the Mac
#ifdef __APPLE__ && __MACH__
    if(geteuid() != 0) {
        AuthorizationItem adminPriv;
        AuthorizationRights adminRights;
        AuthorizationRef adminRightsRef;
        OSStatus result;
        char myPath[MAXPATHLEN];
        uint32_t pathSize = MAXPATHLEN;
        int childStatus;

        adminPriv.name = kAuthorizationRightExecute;
        adminPriv.valueLength = 0;
        adminPriv.value = NULL;
        adminPriv.flags = 0;

        adminRights.count = 1;
        adminRights.items = &adminPriv;

        result = AuthorizationCreate(&adminRights, kAuthorizationEmptyEnvironment, kAuthorizationFlagDefaults | kAuthorizationFlagExtendRights | kAuthorizationFlagInteractionAllowed, &adminRightsRef);

        if(result != errAuthorizationSuccess) {
            fprintf(stderr, "Couldn't Authenticate: %d\n", result);
            exit(1);
        }

        _NSGetExecutablePath(myPath, &pathSize);
        result = AuthorizationExecuteWithPrivileges(adminRightsRef, myPath, kAuthorizationFlagDefaults, NULL, NULL);
        if(result != errAuthorizationSuccess) {
            fprintf(stderr, "Couldn't execute self: %d\n", result);
        }

        waitpid(-1, &childStatus, NULL);
        exit(0);
    }

#endif // __APPLE__ && __MACH__


    QApplication a(argc, argv);
    MainWindow w;
    w.show();

    return a.exec();
}
