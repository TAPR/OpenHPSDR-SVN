#include "BandLimit.h"
BandLimit::BandLimit() {

}

BandLimit::BandLimit(long long min,long long max) {
        min_frequency=min;
        max_frequency=max;
}

long long BandLimit::min() {
    return min_frequency;
}

long long BandLimit::max() {
    return max_frequency;
}
