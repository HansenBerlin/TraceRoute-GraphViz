using Shields.GraphViz.Components;
using Shields.GraphViz.Models;
using Shields.GraphViz.Services;

namespace TraceRoute;

public class GraphFactory
{
    private readonly string _path;
    private readonly string _fileNameCombined;
    private readonly string _fileNameSingle;

    public GraphFactory(string path, string fileNameSingleSingle, string fileNameCombined)
    {
        _path = path;
        _fileNameCombined = fileNameCombined;
        _fileNameSingle = fileNameSingleSingle;
    }

    public async Task Create(List<List<string>> ipLists)
    {
        List<Statement> stm = new();

        foreach (var l in ipLists)
        {
            for (int i = 0; i < l.Count - 1; i++)
            {
                stm.Add(EdgeStatement.For(l[i], l[i + 1]));
            }
        }

        await Save(stm, _fileNameCombined);
    }
    
    public async Task Create(List<string> ipList)
    {
        List<Statement> stm = new();
        for (int i = 0; i < ipList.Count - 1; i++)
        {
            stm.Add(EdgeStatement.For(ipList[i], ipList[i + 1]));
        }

        await Save(stm, _fileNameSingle);
    }

    private async Task Save(List<Statement> stm, string fileName)
    {
        Graph graph = Graph.Undirected.AddRange(stm);

        IRenderer renderer = new Renderer(_path);
        await using Stream file = File.Create(fileName);
        await renderer.RunAsync(
            graph, file,
            RendererLayouts.Dot,
            RendererFormats.Png,
            CancellationToken.None);
    }
}