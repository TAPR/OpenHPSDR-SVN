#include <stdio.h>
#include <string.h>
#include <ctype.h>

#define DELIMS "()+-\" "

#define TOKEN_UNDEFINED -1
#define TOKEN_QUOTE 0
#define TOKEN_LPAREN 1
#define TOKEN_RPAREN 2
#define TOKEN_PLUS 3
#define TOKEN_MINUS 4
#define TOKEN_NUMBER 5
#define TOKEN_RESULT 6
#define TOKEN_FORWARD 7
#define TOKEN_REVERSE 8
#define TOKEN_EOS 9

char* parse_string;
int parse_length;
int parse_index;

char token_string[128];
int token_index;
int token;

int result_stack[10];
int stack_index=-1;

int error=0;

int (*getResult)(int) = NULL;
int (*getForward)(int) = NULL;
int (*getReverse)(int) = NULL;

void simple_expression();

int is_number(char* string) {
    int result=0;
    int len=strlen(string);
    int i;
    for(i=0;i<len;i++) {
        if(isdigit(string[i])) {
            result=result*10;
            result=result+(string[i]-'0');
        } else {
            result=-1;
            break;
        } 
    }
    return result;
}

void get_token(char* string) {
    if(string!=NULL) {
        parse_string=string;
        parse_length=strlen(string);
        parse_index=0;
    }
    
    while((parse_index<parse_length) && parse_string[parse_index]==' ') {
        parse_index++;
    }

    token_index=0;

    if(parse_index>=parse_length) {
        token=TOKEN_EOS;
    } else {
        switch(parse_string[parse_index]) {
            case '(':
                token_string[token_index++]=parse_string[parse_index++];
                token=TOKEN_LPAREN;
                break;
            case ')':
                token_string[token_index++]=parse_string[parse_index++];
                token=TOKEN_RPAREN;
                break;
            case '"':
                token_string[token_index++]=parse_string[parse_index++];
                token=TOKEN_QUOTE;
                break;
            case '+':
                token_string[token_index++]=parse_string[parse_index++];
                token=TOKEN_PLUS;
                break;
            case '-':
                token_string[token_index++]=parse_string[parse_index++];
                token=TOKEN_MINUS;
                break;
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                token_string[token_index++]=parse_string[parse_index++];
                while((parse_index<parse_length) && isdigit(parse_string[parse_index])) {
                    token_string[token_index++]=parse_string[parse_index++];
                }
                token=TOKEN_NUMBER;
                break;

            case '$':
                token_string[token_index++]=parse_string[parse_index++];
                if(parse_string[parse_index]=='F') {
                    token_index++;
                    while((parse_index<parse_length) && isdigit(parse_string[parse_index])) {
                        token_string[token_index++]=parse_string[parse_index++];
                    }
                    token=TOKEN_FORWARD;
                } else if(parse_string[parse_index]=='R') {
                    token_index++;
                    while((parse_index<parse_length) && isdigit(parse_string[parse_index])) {
                        token_string[token_index++]=parse_string[parse_index++];
                    }
                    token=TOKEN_REVERSE;
                } else {
                    while((parse_index<parse_length) && isdigit(parse_string[parse_index])) {
                        token_string[token_index++]=parse_string[parse_index++];
                    }
                    token=TOKEN_RESULT;
                }
                break;
            default:
                token_string[token_index++]=parse_string[parse_index++];
                token=TOKEN_UNDEFINED;
                break;
        }
    }
    token_string[token_index]=0;

//fprintf(stderr,"get_token: %d: %s\n",token,token_string);
}

/*
<expr>:
    <simple expr> | "<simple expr>"

<simple expr>:
    <term> | <sign><term> | <simple expr> <operator> <simple expr>

<sign>:
     + | - 

<term>:
     <unsigned number> | <variable> | (<simple expr)

<operator>:
     + | -

<variable>:
     $<test number>
*/

void push(int value) {
    stack_index++;
    result_stack[stack_index]=value;
//fprintf(stderr,"push: index=%d value=%d\n",stack_index,value);
}

int pop() {
    int value;
    value=result_stack[stack_index];
//fprintf(stderr,"pop: index=%d value=%d\n",stack_index,value);
    stack_index--;
    return value;
}

void add() {
//fprintf(stderr,"add\n");
    int b=pop();
    int a=pop();
    push(a+b);
}

void subtract() {
//fprintf(stderr,"subtract\n");
    int b=pop();
    int a=pop();
    push(a-b);
}

void negate() {
//fprintf(stderr,"negate\n");
    int a;
    a=pop();
    a=0-a;
    push(a);
}

void term() {

    int result_id;
//fprintf(stderr,"term:\n");
    switch(token) {
        case TOKEN_NUMBER:
            push(atoi(token_string));
            get_token(NULL);
            break;
        case TOKEN_RESULT:
            result_id=atoi(&token_string[1]);
            push(getResult(result_id));
            get_token(NULL);
            break;
        case TOKEN_FORWARD:
            result_id=atoi(&token_string[1]);
            push(getForward(result_id));
            get_token(NULL);
            break;
        case TOKEN_REVERSE:
            result_id=atoi(&token_string[1]);
            push(getReverse(result_id));
            get_token(NULL);
            break;
        case TOKEN_LPAREN:
            get_token(NULL);
            simple_expression();
            if(token==TOKEN_RPAREN) {
                get_token(NULL);
            } else {
                fprintf(stderr,"Expected ')'\n");
                error++;
            }
            break;
        default:
            push(0);
            fprintf(stderr,"expected <term> numeric value or result variable\n");
            error++;
            break;
    }
}

void simple_expression() {
//fprintf(stderr,"simple_expression:\n");
    switch(token) {
        case TOKEN_PLUS:
            get_token(NULL);
            term();
            break;
        case TOKEN_MINUS:
            get_token(NULL);
            term();
            negate();
            break;
        default:
            term();
            break;
    }
    while(token==TOKEN_PLUS ||  token==TOKEN_MINUS) {
        if(token==TOKEN_PLUS) {
            get_token(NULL);
            term();
            add();
        } else if(token==TOKEN_MINUS) {
            get_token(NULL);
            term();
            subtract();
        }
    }
}

int evaluate(char* expression,int (*result)(int),int (*forward)(int),int (*reverse)(int)) {
    int value;
//fprintf(stderr,"evaluate: %s\n",expression);
    getResult=result;
    getForward=forward;
    getReverse=reverse;
    stack_index=-1;
    get_token(expression);
    switch(token) {
        case TOKEN_QUOTE:
            get_token(NULL);
            simple_expression();
            if(token!=TOKEN_QUOTE) {
                fprintf(stderr,"Expected trailing quote\n");
                error++;
            }
            break;
        default:
            simple_expression();
            break;
    }
    if(token!=TOKEN_EOS) {
        fprintf(stderr,"Evaluate: Unexpected trailing tokens:  %d: %s\n",token,token_string);
        error++;
    }

    if(stack_index!=0) {
        fprintf(stderr,"evaluate: expected stack index to be 0 but is %d\n",stack_index);
        value=0;
    } else {
        value=pop();
        fprintf(stderr,"evaluate: result=%d\n",value);
    }

    return value;

}




/*
int get_result(int id) {
fprintf(stderr,"get_result: %d\n",id);
    return id;
}


void main(int argc,char**argv) {
    if(argc!=2) {
        fprintf(stderr,"Usage: expression \"<expression>\"\n");
    } else {
        fprintf(stderr,"result=%d\n",evaluate(argv[1],get_result));
    }
}
*/
