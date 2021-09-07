
typedef struct VertexStruct *Vertex;
typedef struct GraphRep *Graph;

Graph CreateGraph(int nV, char **names);
void DestroyGraph(Graph graph);

void InsertEdge(Graph graph, int vIndex, int wIndex, int weight);
void RemoveEdge(Graph graph, int vIndex, int wIndex);

void ShowGraph(Graph graph);
int GetSize(Graph graph);
int *GetIncoming(Graph graph, int vIndex, int *nIncoming);
int *GetOutgoing(Graph graph, int vIndex, int *nOutgoing);

double *GetAllPR(Graph graph);
void SetAllPR(Graph graph, double *PR);
double GetPR(Graph graph, int vIndex);
void SetPR(Graph graph, int vIndex, double PR);