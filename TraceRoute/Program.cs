using TraceRoute;

const bool print = true;
const int timeout = 5000;
const int maxHops = 20;
const string path = @"C:\Program Files\Graphviz\bin";
const string filenameSingle = "singleTraceRoute.png";
const string filenameCombined = "combinedTraceRoute.png";

List<string> ips = new List<string>
{
    "20.112.52.29",         // MS
    "142.250.186.174",      // Google
    "194.94.22.19"          // HWR
};

var tr = new TraceRt();
var tasks= ips.Select(ip => tr.TracertIpsAsync(ip, maxHops, timeout, print)).Cast<Task>().ToList();
await Task.WhenAll(tasks);
var results= tasks.Select(task => ((Task<List<string>>) task).Result).ToList();

// Combined graph
var graphCreator = new GraphFactory(path, filenameSingle, filenameCombined);
await graphCreator.Create(results);

// Single graph
var result = await tr.TracertIpsAsync(ips[0], maxHops, timeout, print);
await graphCreator.Create(result);




