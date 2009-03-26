// filter.h

#define filter6000 0
#define filter4000 1
#define filter2600 2
#define filter2100 3
#define filter1000 4
#define filter500 5
#define filter250 6
#define filter100 7
#define filter50 8
#define filter25 9
#define filterVar1 10
#define filterVar2 11

int filter;

int filterLow;
int filterHigh;

int txFilterLowCut;
int txFilterHighCut;

int filterVar1Low;
int filterVar1High;
int filterVar2Low;
int filterVar2High;


GtkWidget* buildFilterUI();
void filterSaveState();
void filterRestoreState();
void setFilter(int filter);
void setTxFilters();
