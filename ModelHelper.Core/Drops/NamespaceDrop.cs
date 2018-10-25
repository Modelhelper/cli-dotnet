using ModelHelper.Core.Project;
using DotLiquid;

namespace ModelHelper.Core.Drops
{
    public class NamespaceDrop : Drop
    {
        public string Model { get; }
        public string Data { get; }
        public string Interfaces { get; }
        public string Controllers { get; }

        public NamespaceDrop(IProject project)
        {
            //Model = !string.IsNullOrEmpty(project.Models?.Namespace)
            //    ? project.Models.Namespace
            //    : "Models";

            //Interfaces = !string.IsNullOrEmpty(project.Interfaces?.Namespace)
            //    ? project.Interfaces.Namespace
            //    : "Interfaces";

            //Data = !string.IsNullOrEmpty(project.Repositories?.Namespace)
            //    ? project.Repositories.Namespace
            //    : "Data";

            //Controllers = !string.IsNullOrEmpty(project.Controllers?.Namespace)
            //    ? project.Controllers.Namespace
            //    : "Controllers";
        }
        public NamespaceDrop(string model, string data, string interfaces, string controllers)
        {
            Model = model;
            Data = data;
            Interfaces = interfaces;
            Controllers = controllers;
        }
    }
}