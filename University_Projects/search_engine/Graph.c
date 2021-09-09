#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>
#include <assert.h>
#include <string.h>

#include "Graph.h"

static bool validV(Graph graph, int vIndex);

typedef struct VertexStruct{
    double PR;
    char *name;
} VertexStruct;

/*typedef struct {
    Vertex v;
    Vertex w;
    int weight;
} Edge;*/

typedef struct GraphRep{
    Vertex *vertecies;
    int **edges;
    int nV;
    int nE;
} GraphRep;

static bool validV(Graph graph, int vIndex);

Graph CreateGraph(int nV, char **names) {

    assert(nV > 0);

    Graph graph = malloc(sizeof(GraphRep));
    assert(graph != NULL);
    graph->nV = nV;
    graph->nE = 0;

    graph->edges = calloc(nV, sizeof(int *));
    assert(graph->edges != NULL);
    for (int i = 0; i < nV; i++) {
        graph->edges[i] = calloc(nV, sizeof(int));
        assert(graph->edges[i] != NULL);
    }

    graph->vertecies = malloc(nV * sizeof(Vertex));
    assert(graph->vertecies != NULL);
    for(int i = 0; i < nV; i++) {
        graph->vertecies[i] = malloc(sizeof(VertexStruct));
        graph->vertecies[i]->PR = -1;
        graph->vertecies[i]->name = malloc(strlen(names[i]));
        strcpy(graph->vertecies[i]->name, names[i]);
    }
    
    return graph;
}

void DestroyGraph(Graph graph) {

    for (int i = 0; i < graph->nV; i++) free(graph->vertecies[i]->name);
    free(graph->vertecies);

    for (int i = 0; i < graph->nE; i++) free(graph->edges[i]);
    free(graph->edges);

    free(graph);
}

void InsertEdge(Graph graph, int vIndex, int wIndex, int weight) {
    assert(graph != NULL && validV(graph, vIndex) && validV(graph, wIndex) && weight > 0);
    if (graph->edges[vIndex][wIndex] == 0) graph->nE++;
    graph->edges[vIndex][wIndex] = weight;
}

void RemoveEdge(Graph graph, int vIndex, int wIndex) {
    assert(graph != NULL && validV(graph, vIndex) && validV(graph, wIndex));
    if (graph->edges[vIndex][wIndex] == 0) return;
    graph->edges[vIndex][wIndex] = 0;
    graph->nE--;
}

//edited version of the 'ShowGraph' function from week 08 lab exersises.
void ShowGraph(Graph graph) {
    assert (graph != NULL);
	printf ("#vertices=%d, #edges=%d\n\n", graph->nV, graph->nE);
	int v, w;
	for (v = 0; v < graph->nV; v++) {
		printf ("%d %s\n", v, graph->vertecies[v]->name);
		for (w = 0; w < graph->nV; w++) {
			if (graph->edges[v][w]) {
				printf ("\t%s (%d)\n", graph->vertecies[w]->name, graph->edges[v][w]);
			}
		}
		printf ("\n");
	}
}

int GetSize(Graph graph) {
    assert(graph != NULL);
    return graph->nV;
}

int *GetIncoming(Graph graph, int vIndex, int *nIncoming) {
    
    *nIncoming = 0;
    int *incoming = malloc(graph->nV * sizeof(int));
    for (int i = 0; i < graph->nV; i++) {

        if (graph->edges[i][vIndex] != 0 && i != vIndex) {
            incoming[*nIncoming] = i;
            (*nIncoming)++;
        }

    }

    return incoming;
}

int *GetOutgoing(Graph graph, int vIndex, int *nOutgoing) {
    
    *nOutgoing = 0;
    int *outgoing = malloc(graph->nV * sizeof(int));
    for (int i = 0; i < graph->nV; i++) {

        if (graph->edges[vIndex][i] != 0 && i != vIndex) {
            outgoing[*nOutgoing] = i;
            (*nOutgoing)++;
        }
    }

    return outgoing;
}

void SetPR(Graph graph, int vIndex, double PR) {
    assert(graph != NULL);
    graph->vertecies[vIndex]->PR = PR;
}

double *GetAllPR(Graph graph) {
    double *PR = malloc(graph->nV * sizeof(double));
    for (int i = 0; i < graph->nV; i++) PR[i] = graph->vertecies[i]->PR;
    return PR;
}

double GetPR(Graph graph, int vIndex) {
    assert(graph != NULL && validV(graph, vIndex) == true);
    return graph->vertecies[vIndex]->PR;
}

//private functions:

static bool validV(Graph graph, int vIndex) {
    return (vIndex < graph->nV && vIndex >= 0);
}