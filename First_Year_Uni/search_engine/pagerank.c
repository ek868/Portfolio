#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <string.h>
#include <errno.h>
#include <math.h>
#include <assert.h>

#include "Graph.h"

#define INIT_MALLOC_LEN         10
#define MALLOC_LEN_INCREMENT    10
#define MAX_WORD_LENGTH         256
#define DID_NOT_FIND            -1

static char **read_file(FILE *file, int *nWords);
static bool valid_args(int argc, char **argv);
static bool is_float(char *str);
static FILE *section1(FILE *url);
static int find(char **urls, int nUrls, char *url);
Graph ConstructWeb(char **urls, int nUrls);
void CalculatePageRank(Graph web, double d, double diffPR, int maxIterations);
static double Win(Graph web, int vIndex, int wIndex);
static double Wout(Graph web, int vIndex, int wIndex);

int main(int argc, char **argv) {

    if (!valid_args(argc, argv)) { fprintf(stderr, "ERROR: Invalid arguments.\nUsage: ./pagerank [d] [diffPR] [maxIterations]\n"); exit(1); }

    FILE *file = fopen("collection.txt", "r");
    if (file == NULL) { fprintf(stderr, "ERROR: no 'collection.txt' in directory\n"); exit(errno); }

    int nUrls = 0;
    char **urls = read_file(file, &nUrls);

    Graph web = ConstructWeb(urls, nUrls);
    for(int i = 0; i < nUrls; i++) SetPR(web, i, (double)1/(double)nUrls);
    CalculatePageRank(web, atof(argv[1]), atof(argv[2]), atoi(argv[3]));

    ShowGraph(web);
    if (fclose(file) != 0) { fprintf(stderr, "ERROR: can not close 'collection.txt'\n"); exit(errno); }

    return 0;

}

Graph ConstructWeb(char **urls, int nUrls) {

    Graph web = CreateGraph(nUrls, urls);

    for (int i = 0; i < nUrls; i++) {

        char filename[MAX_WORD_LENGTH];
        strcpy(filename, urls[i]);
        strcat(filename, ".txt");

        FILE *url = fopen(filename, "r");
        if (url == NULL) { fprintf(stderr, "ERROR: no '%s' in directory\n", filename); exit(errno); }
        
        int nLinks = 0;
        url = section1(url);
        if (url == NULL) { fprintf(stderr, "ERROR: no '#start Section-1' in '%s'\n", filename); exit(errno); }
        char **links = read_file(url, &nLinks);
        if (fclose(url) != 0) { fprintf(stderr, "ERROR: can not close '%s'\n", filename); exit(errno); }

        for (int j = 0; j < nLinks; j++) {
            int index = find(urls, nUrls, links[j]);
            if (index == DID_NOT_FIND) { fprintf(stderr, "ERROR: Invalid link '%s'\n", links[j]); continue; }
            InsertEdge(web, i, index, 1);
        }
    }

    return web;
}

void CalculatePageRank(Graph web, double d, double diffPR, int maxIterations) {
    int N = GetSize(web);
    int iteration = 0;
    double diff = diffPR;
    double *PR = GetAllPR(web);
    double constant = (((1 - d)/N) + d);
    while (iteration < maxIterations && diff >= diffPR) {

        for (int i = 0; i < N; i++) {
            double sum = 0;
            int nIncoming;
            int *incoming = GetIncoming(web, i, &nIncoming);
            for (int j = 0; j < nIncoming; j++) {
                sum += GetPR(web, incoming[j]) * Win(web, i, incoming[j]) * Wout(web, i, incoming[j]);
            }
            PR[i] = constant * sum;
        }

        for (int i = 0; i < N; i++) {
            double oldPR = GetPR(web, i);
            diff += ((oldPR - PR[i]) < 0 ? -1 * (oldPR - PR[i]) : (oldPR - PR[i]));
        }

        for (int i = 0; i < N; i++) SetPR(web, i, PR[i]);

        iteration++;
    }
}

//Private functions

static char **read_file(FILE *file, int *nWords) {

    char **words = malloc(INIT_MALLOC_LEN * sizeof(char *));
    for (int i = 0; i < INIT_MALLOC_LEN; i++) words[i] = malloc(MAX_WORD_LENGTH);
    int nMalloced = INIT_MALLOC_LEN;

    while (fscanf(file, "%s", words[*nWords]) > 0 && strcmp(words[*nWords], "#end") != 0) {
        if (*nWords == nMalloced) {
            words = realloc (words, (nMalloced + MALLOC_LEN_INCREMENT) * sizeof(char *));
            for (int i = nMalloced; i < nMalloced + MALLOC_LEN_INCREMENT; i++) words[i] = malloc(MAX_WORD_LENGTH);
            nMalloced += MALLOC_LEN_INCREMENT;
        }
        (*nWords)++;
    }

    return words;
}

static bool valid_args(int argc, char **argv) {
    if (argc != 4) return false;
    for (int i = 1; i < 4; i++) {
        if (!is_float(argv[i])) return false;
    }
    return true;
}

static bool is_float(char *str) {
    for (int i = 0; str[i] != '\0'; i++) {
        if ((str[i] < '0' || str[i] > '9') && str[i] != '.') return false;
    }
    return true;
}

static FILE *section1(FILE *url) {
    char tmp[MAX_WORD_LENGTH];
    while (fscanf(url, "%s", tmp) > 0) {
        if (strcmp(tmp, "#start") == 0) {
            
            if (fscanf(url, "%s", tmp) > 0
            && strcmp(tmp, "Section-1") == 0) {

                return url;
            }
        }
    }
    return NULL;
}

static int find(char **urls, int nUrls, char *url) {
    for (int i = 0; i < nUrls; i++) {
        if (strcmp(urls[i], url) == 0) return i;
    }
    return DID_NOT_FIND;
}

static double Win(Graph web, int vIndex, int wIndex) {
    
    int divisor = 0;
    int nInW;
    free(GetIncoming(web, wIndex, &nInW));

    int nOutV;
    int *outV = GetOutgoing(web, vIndex, &nOutV);

    for (int i = 0; i < nOutV; i++) {
        int nIn;
        free(GetIncoming(web, outV[i], &nIn));
        divisor += nIn;
    }

    free(outV);
    assert(divisor != 0);
    return ((double)nInW/(double)divisor);
}

static double Wout(Graph web, int vIndex, int wIndex) {

    int divisor = 0;
    int nOutW;
    free(GetOutgoing(web, wIndex, &nOutW));

    int nOutV;
    int *outV = GetOutgoing(web, vIndex, &nOutV);

    for (int i = 0; i < nOutV; i++) {
        int nOut;
        free(GetOutgoing(web, outV[i], &nOut));
        divisor += nOut;
    }

    free(outV);
    assert(divisor != 0);
    return ((double)nOutW/(double)divisor);
}