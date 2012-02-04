#ifndef G711U_H
#define G711U_H

class G711u
{
public:
    G711u();
    unsigned char encode(short sample);
    short decode(unsigned char sample);
private:
    short decodetable[256];
    unsigned char encodetable[65536];
};

#endif // G711U_H
