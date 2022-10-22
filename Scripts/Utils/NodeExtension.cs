using Godot;
using System;
using Godot.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Scripts.Utils
{
    public static class NodeExtension
    {
        /// <summary>
        /// Load a scene smartly. Takes a path to a node to instanciate.
        /// </summary>
        /// <typeparam name="T">The type to return. T must inherit node.</typeparam>
        /// <param name="path">The path to load.</param>
        /// <returns></returns>
        public static T SmartLoader<T>(string path) where T : Node
        {
            try
            {
                var resource = ResourceLoader.Load(path);
                PackedScene packedScene = (PackedScene)resource;
                var instance = packedScene.Instantiate<T>();
                return instance;
            }
            catch (Exception exception)
            {
                GD.PrintErr($"Exception while smart loading a scene : {exception.Message}");
                throw exception;
            }
        }

        /// <summary>
        /// Search for children of Type T and return them in a new Array.
        /// </summary>
        /// <typeparam name="T">The type to return. T must inherit node.</typeparam>
        /// <param name="node">The node from which we will start searching.</param>
        /// <returns></returns>
        public static List<T> FindChildrenOfType<T>(this Node node) where T : Node
        {
            var children = node.GetChildren();
            var nodes = new Array<Node>(children);
            var uncasted = nodes.Where(_ => _.GetType() == typeof(T));

            List<T> result = new List<T>();
            foreach (var toCast in uncasted)
            {
                result.Add((T)toCast);
            }

            return result;
        }
    }
}