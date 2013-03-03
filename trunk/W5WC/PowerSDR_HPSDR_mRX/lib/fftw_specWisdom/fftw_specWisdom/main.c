#include <stdio.h>
#include <fftw3.h>
#include <math.h>
#include <string.h>

#ifndef M_PI
#define M_PI 3.1415926535897932
#endif

int main(int argc, char **argv) {
	fftw_plan tplan;
	FILE *wisdom_file;
	char *wisdom_string;
	double *inr;
	char fullpath[1024];
	int exit(int);
    int psize = 64;
    fftw_complex *in = ( fftw_complex* )fftw_malloc( sizeof( fftw_complex ) * 5 * 262144);
    fftw_complex *out = ( fftw_complex* )fftw_malloc( sizeof( fftw_complex ) * 5 * 262144);
    memset( in, 0, sizeof(fftw_complex) * 5 * 262144);
    memset( out, 0, sizeof(fftw_complex) * 5 * 262144);

	inr = *in;

	if (argc<2) {
		fprintf(stderr,"No path provided to write the file\n");
		exit(1);
	}
	fprintf(stderr, "%s\n", argv[1]);
	strcpy(fullpath,argv[1]);
	strcat(fullpath,"specWisdom");
	//	fprintf(stderr,"fullpath=%s\n",fullpath);
	//	exit(0);
	if ((wisdom_file = fopen(fullpath,"w")) != NULL) {
		fprintf(stderr,"specWisdom file successfully opened\n");
	    fprintf(stderr,"Creating (Real) fft's up to 262144\n");
		fprintf(stderr,"64 Real\n");
		tplan = fftw_plan_dft_r2c_1d(64,inr,out,FFTW_PATIENT);
	    fftw_execute(tplan);		
		fftw_destroy_plan(tplan);
		fprintf(stderr,"128 Real\n");
		tplan = fftw_plan_dft_r2c_1d(128,inr,out,FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);
		fprintf(stderr,"256 Real\n");
		tplan = fftw_plan_dft_r2c_1d(256,inr,out,FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);
		fprintf(stderr,"512 Real\n");
		tplan = fftw_plan_dft_r2c_1d(512,inr,out,FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);
		fprintf(stderr,"1024 Real\n");
		tplan = fftw_plan_dft_r2c_1d(1024,inr,out,FFTW_PATIENT);
		fftw_destroy_plan(tplan);
		fprintf(stderr,"2048 Real\n");
		tplan = fftw_plan_dft_r2c_1d(2048,inr,out,FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);
		fprintf(stderr,"4096 Real\n");
		tplan = fftw_plan_dft_r2c_1d(4096,inr,out,FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);
		fprintf(stderr,"8192 Real\n");
		tplan = fftw_plan_dft_r2c_1d(8192,inr,out,FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);
		fprintf(stderr,"16384 Real\n");
		tplan = fftw_plan_dft_r2c_1d(16384,inr,out,FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);
		fprintf(stderr,"32768 Real\n");
		tplan = fftw_plan_dft_r2c_1d(32768,inr,out,FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan); 
		fprintf(stderr,"65536 Real\n");
		tplan = fftw_plan_dft_r2c_1d(65536,inr,out,FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan); 
		fprintf(stderr,"131072 Real\n");
		tplan = fftw_plan_dft_r2c_1d(131072,inr,out,FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan); 
		fprintf(stderr,"262144 Real\n");
		tplan = fftw_plan_dft_r2c_1d(262144,inr,out,FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan); 
				
		fprintf(stderr,"Creating (Complex) fft's up to 262144\n");	
		fprintf(stderr,"64 Forward\n");
		tplan = fftw_plan_dft_1d(64,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);

		fprintf(stderr,"128 Forward\n");
		tplan = fftw_plan_dft_1d(128,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);

		fprintf(stderr,"256 Forward\n");
		tplan = fftw_plan_dft_1d(256,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);

		fprintf(stderr,"512 Forward\n");
		tplan = fftw_plan_dft_1d(512,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);

		fprintf(stderr,"1024 Forward\n");
		tplan = fftw_plan_dft_1d(1024,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);

		fprintf(stderr,"2048 Forward\n");
		tplan = fftw_plan_dft_1d(2048,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);

		fprintf(stderr,"4096 Forward\n");
		tplan = fftw_plan_dft_1d(4096,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);

		fprintf(stderr,"8192 Forward\n");
		tplan = fftw_plan_dft_1d(8192,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);

		fprintf(stderr,"16384 Forward\n");
		tplan = fftw_plan_dft_1d(16384,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);

		fprintf(stderr,"32768 Forward\n");
		tplan = fftw_plan_dft_1d(32768,in,out,FFTW_FORWARD,
			    FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);

		fprintf(stderr,"65536 Forward\n");
		tplan = fftw_plan_dft_1d(65536,in,out,FFTW_FORWARD,
			    FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);

		fprintf(stderr,"131072 Forward\n");
		tplan = fftw_plan_dft_1d(131072,in,out,FFTW_FORWARD,
			    FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);

		fprintf(stderr,"262144 Forward\n");
		tplan = fftw_plan_dft_1d(262144,in,out,FFTW_FORWARD,
			    FFTW_PATIENT);
		fftw_execute(tplan);
		fftw_destroy_plan(tplan);
		
		fftw_free(in);
		fftw_free(out);
		fprintf(stderr,"Finished computing, exporting wisdom\n");
		fflush(stderr);
		wisdom_string = fftw_export_wisdom_to_string();
		fprintf(wisdom_file,"%s",wisdom_string);
		fclose(wisdom_file);
		fprintf(stderr,"%s\n",wisdom_string);
		fprintf(stderr,"Done!\n");

	} else 
		fprintf(stderr,"Could not create the wisdom file.\n");
	//fprintf(stderr,"Press the Enter key to close."),gets(dummy);
}