//
//  MainViewController.m
//  HPSDR
//
//  Created by John Melton on 07/07/2009.
//  Copyright __MyCompanyName__ 2009. All rights reserved.
//

#include <AudioToolbox/AudioQueue.h>

#import "MainViewController.h"
#import "MainView.h"
#import "band.h"
#import "samples.h"
#import "connection.h"
#import "audiostream.h"

@implementation MainViewController

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil {
    if (self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil]) {
        // Custom initialization
    }
    return self;
}


-(void) onTimer {
	// force view to be redrawn
	//fprintf(stderr,"onTimer\n");
	//checkDragged();
	[[self view] dragged];
	
	sendCommand("getSpectrum 480");
	
	if(currentBand!=displayedBand) {
		if(displayedBand!=-1) {
			[self resetBandButton:displayedBand];
		}
		displayedBand=currentBand;
		[self setBandButton:displayedBand];
	}
	
	[[self view] refresh];
}

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
    [self resetTimer];
	
	audiostream_init();
	makeConnection();
	
	setFrequency(7048000);
	setMode(0);
	setFilter(-2850,-150);
	setGain(30);
	
}

- (void)resetTimer {
	[timer invalidate];
	fprintf(stderr,"resetTimer: fps=%f\n",fps);
	timer=[NSTimer scheduledTimerWithTimeInterval:1.0/fps target:self selector:@selector(onTimer) userInfo:nil repeats:YES];
	
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
return (interfaceOrientation == UIInterfaceOrientationLandscapeLeft);
}
 


- (void)flipsideViewControllerDidFinish:(FlipsideViewController *)controller {
    
	[self dismissModalViewControllerAnimated:YES];
}

- (IBAction)showInfo {    
	
	FlipsideViewController *controller = [[FlipsideViewController alloc] initWithNibName:@"FlipsideView" bundle:nil];
	controller.delegate = self;
	
	controller.modalTransitionStyle = UIModalTransitionStyleFlipHorizontal;
	[self presentModalViewController:controller animated:YES];
	
	[controller release];
}

- (IBAction)band160 {
	fprintf(stderr,"band160\n");
	setFrequency(1850000);
	setMode(0);
	setFilter(-2850,-150);
}

- (IBAction)band80 {
	fprintf(stderr,"band80\n");
	setFrequency(3790000);
	setMode(0);
	setFilter(-2850,-150);
}

- (IBAction)band60 {
	fprintf(stderr,"band60\n");
	setFrequency(5371500);
	setMode(0);
	setFilter(-2850,-150);
}

- (IBAction)band40 {
	fprintf(stderr,"band40\n");
	setFrequency(7048000);
	setMode(0);
	setFilter(-2850,-150);
}

- (IBAction)band30 {
	fprintf(stderr,"band30\n");
	setFrequency(10135600);
	setMode(1);
	setFilter(150,2850);
}

- (IBAction)band20 {
	fprintf(stderr,"band20\n");
	setFrequency(14260000);
	setMode(1);
	setFilter(150,2850);
}

- (IBAction)band17 {
	fprintf(stderr,"band17\n");
	setFrequency(18118900);
	setMode(1);
	setFilter(150,2850);
}

- (IBAction)band15 {
	fprintf(stderr,"band15\n");
	setFrequency(21300000);
	setMode(1);
	setFilter(150,2850);
}

- (IBAction)band12 {
	fprintf(stderr,"band12\n");
	setFrequency(24910000);
	setMode(1);
	setFilter(150,2850);
}

- (IBAction)band10 {
	fprintf(stderr,"band10\n");
	setFrequency(2850000);
	setMode(1);
	setFilter(150,2850);
}

- (IBAction)band6 {
	fprintf(stderr,"band6\n");
	setFrequency(50200000);
	setMode(1);
	setFilter(150,2850);
}

- (IBAction)bandGen {
	fprintf(stderr,"bandGen\n");
	setFrequency(909000);
	setMode(6);
	setFilter(-4000,4000);
}

- (IBAction)bandWWV {
	fprintf(stderr,"bandWWV\n");
	setFrequency(5000000);
	setMode(10);
	setFilter(-4000,4000);
}


/*
 // Override to allow orientations other than the default portrait orientation.
 - (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
 // Return YES for supported orientations
 return (interfaceOrientation == UIInterfaceOrientationPortrait);
 }
 */

- (void)didReceiveMemoryWarning {
	// Releases the view if it doesn't have a superview.
    [super didReceiveMemoryWarning];
	
	// Release any cached data, images, etc that aren't in use.
}

- (void)viewDidUnload {
	// Release any retained subviews of the main view.
	// e.g. self.myOutlet = nil;
}


- (void)dealloc {
    [super dealloc];
}

- (void)setBandButton: (int) band {
	
	fprintf(stderr,"setBandButton: %d\n",band);
	switch(band) {
		case BAND160:
			button160.alpha=0.8;
			break;
		case BAND80:
			button80.alpha=0.8;
			break;
		case BAND60:
			button60.alpha=0.8;
			break;
		case BAND40:
			button40.alpha=0.8;
			break;
		case BAND30:
			button30.alpha=0.8;
			break;
		case BAND20:
			button20.alpha=0.8;
			break;
		case BAND17:
			button17.alpha=0.8;
			break;
		case BAND15:
			button15.alpha=0.8;
			break;
		case BAND12:
			button12.alpha=0.8;
			break;
		case BAND10:
			button10.alpha=0.8;
			break;
		case BAND6:
			button6.alpha=0.8;
			break;
		case BANDGen:
			buttonGen.alpha=0.8;
			break;
		case BANDWWV:
			buttonWWV.alpha=0.8;
			break;
	}
}

- (void) resetBandButton: (int) band {
	fprintf(stderr,"resetBandButton: %d\n",band);
	switch(band) {
		case BAND160:
			button160.alpha=0.5;
			break;
		case BAND80:
			button80.alpha=0.5;
			break;
		case BAND60:
			button60.alpha=0.5;
			break;
		case BAND40:
			button40.alpha=0.5;
			break;
		case BAND30:
			button30.alpha=0.5;
			break;
		case BAND20:
			button20.alpha=0.5;
			break;
		case BAND17:
			button17.alpha=0.5;
			break;
		case BAND15:
			button15.alpha=0.5;
			break;
		case BAND12:
			button12.alpha=0.5;
			break;
		case BAND10:
			button10.alpha=0.5;
			break;
		case BAND6:
			button6.alpha=0.5;
			break;
		case BANDGen:
			buttonGen.alpha=0.5;
			break;
		case BANDWWV:
			buttonWWV.alpha=0.5;
			break;
	}
			
}

@end
