using Microsoft.AspNetCore.Mvc;
using Sotis2.Models.Graph;
using Sotis2.Models.Graph.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Services
{
    public static class Converters
    {
        public static GraphDTO Convert(Graph source)
        {
            if (source == null)
            {
                return null;
            }

            return new GraphDTO
            {
                Nodes = ConvertNodes(source.Nodes),
                Edges = ConvertEdges(source.Edges)
            };
        }

        private static IList<NodeDto> ConvertNodes(IList<Node> source)
        {
            if (source == null)
            {
                return null;
            }

            return source
                .Select(x => ConvertNode(x))
                .ToList();
        }

        private static NodeDto ConvertNode(Node source)
        {
            if (source == null)
            {
                return null;
            }

            return new NodeDto
            {
                Label = source.Name,
                Id = source.Id,
                Group = source.Name
            };
        }

        private static IList<EdgeDto> ConvertEdges(IList<Edge> source)
        {
            if (source == null)
            {
                return null;
            }

            return source
                .Select(x => ConvertEdge(x))
                .ToList();
        }

        private static EdgeDto ConvertEdge(Edge source)
        {
            if (source == null)
            {
                return null;
            }

            return new EdgeDto
            {
                Label = source.Name,
                From = source.From,
                To = source.To
            };
        }
    }
}
