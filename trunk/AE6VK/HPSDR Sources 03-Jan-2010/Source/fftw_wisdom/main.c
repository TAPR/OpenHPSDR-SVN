#include <stdio.h>
#include <fftw3.h>
#include <math.h>
#ifndef M_PI
#define M_PI 3.14159265358928
#endif

int main(int argc, char **argv) {
	fftwf_plan ptmp;
	FILE *wisdom_file;
	char *wisdom_string;
	fftwf_complex *in, *out;
	double *inf,*outf;
	in  = (fftwf_complex *)fftwf_malloc(65536*sizeof(fftwf_complex));
	out = (fftwf_complex *)fftwf_malloc(65536*sizeof(fftwf_complex));
	inf  = (double *)in;
	outf = (double *)out;
	if ((wisdom_file = fopen("wisdom","w")) != NULL) {
		fprintf(stderr,"Wisdom file successfully opened\n");
		fprintf(stderr,"Checking forward fft's up to 8192\n");
		fprintf(stderr,"64 Forward\n");
		ptmp = fftwf_plan_dft_1d(64,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		{
			int i;
			for(i=0;i<64;i++) {
				inf[2*i]   = cos(i*M_PI/129.0);
				inf[2*i+1] = sin(i*M_PI/129.0);
			}
			fftwf_execute(ptmp);
			/*for(i=0;i<64;i++) {
				fprintf(stderr,"bin[%3d] =  (%15.10f %15.10f)\n",i,outf[2*i],outf[2*i+1]);
			}*/
		}
		fftwf_destroy_plan(ptmp);
		fprintf(stderr,"128 Forward\n");
		ptmp = fftwf_plan_dft_1d(128,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
		fprintf(stderr,"256 Forward\n");
		ptmp = fftwf_plan_dft_1d(256,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
		fprintf(stderr,"512 Forward\n");
		ptmp = fftwf_plan_dft_1d(512,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
		fprintf(stderr,"1024 Forward\n");
		ptmp = fftwf_plan_dft_1d(1024,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
		fprintf(stderr,"2048 Forward\n");
		ptmp = fftwf_plan_dft_1d(2048,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
		fprintf(stderr,"4096 Forward\n");
		ptmp = fftwf_plan_dft_1d(4096,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
		fprintf(stderr,"8192 Forward\n");
		ptmp = fftwf_plan_dft_1d(8192,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
/*		fprintf(stderr,"16384 Forward\n");
		ptmp = fftwf_plan_dft_1d(16384,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
		fprintf(stderr,"32768 Forward\n");
		ptmp = fftwf_plan_dft_1d(32768,in,out,FFTW_FORWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp); */


		fprintf(stderr,"Checking inverse fft's up to 8192\n");

		fprintf(stderr,"64 Backward\n");
		ptmp = fftwf_plan_dft_1d(64,in,out,FFTW_BACKWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
		fprintf(stderr,"128 Backward\n");
		ptmp = fftwf_plan_dft_1d(128,in,out,FFTW_BACKWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
		fprintf(stderr,"256 Backward\n");
		ptmp = fftwf_plan_dft_1d(256,in,out,FFTW_BACKWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
		fprintf(stderr,"512 Backward\n");
		ptmp = fftwf_plan_dft_1d(512,in,out,FFTW_BACKWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
		fprintf(stderr,"1024 Backward\n");
		ptmp = fftwf_plan_dft_1d(1024,in,out,FFTW_BACKWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
		fprintf(stderr,"2048 Backward\n");
		ptmp = fftwf_plan_dft_1d(2048,in,out,FFTW_BACKWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
		fprintf(stderr,"4096 Backward\n");
		ptmp = fftwf_plan_dft_1d(4096,in,out,FFTW_BACKWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
		fprintf(stderr,"8192 Backward\n");
		ptmp = fftwf_plan_dft_1d(8192,in,out,FFTW_BACKWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
/*		fprintf(stderr,"16384 Backward\n");
		ptmp = fftwf_plan_dft_1d(16384,in,out,FFTW_BACKWARD,
				    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
		fprintf(stderr,"32768 Backward\n");
		ptmp = fftwf_plan_dft_1d(32768,in,out,FFTW_BACKWARD,
			    FFTW_PATIENT);
		fftwf_destroy_plan(ptmp);
*/

		fftwf_free(in);
		fftwf_free(out);
		fprintf(stderr,"Finished computing, exporting wisdom\n");
		fflush(stderr);
		wisdom_string = fftwf_export_wisdom_to_string();
		fprintf(wisdom_file,"%s",wisdom_string);
		fclose(wisdom_file);
		fprintf(stderr,"%s\n",wisdom_string);
		fprintf(stderr,"Done!\n");

	} else 
		fprintf(stderr,"Could not create the wisdom file.\n");
	//fprintf(stderr,"Press the Enter key to close."),gets(dummy);
}