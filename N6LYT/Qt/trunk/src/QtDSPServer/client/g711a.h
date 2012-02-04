#ifndef G711A_H
#define G711A_H

class G711a
{
public:
    G711a();
    unsigned char encode(short sample);
    short decode(unsigned char sample);
private:
    short decodetable[256];
    unsigned char encodetable[65536];
};

#endif // G711A_H
