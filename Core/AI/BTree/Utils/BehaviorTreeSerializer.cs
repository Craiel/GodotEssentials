namespace Craiel.Essentials.Runtime.AI.BTree.Utils
{
    using System;
    using System.Collections.Generic;
    using BTree;
    using Contracts;
    using Data.SBT;
    using Exceptions;

    /// <summary>
    /// Serializer for <see cref="BehaviorStream{T}"/>
    /// </summary>
    /// <typeparam name="T">the type of <see cref="IBlackboard"/> the tree is using</typeparam>
    public class BehaviorTreeSerializer<T>
        where T : IBlackboard
    {
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------

        /// <summary>
        /// Serialize the given tree to string format using Json
        /// </summary>
        /// <param name="tree">the tree to serialize</param>
        /// <returns>the string data for the tree</returns>
        /// <exception cref="SerializationException">if any data is not able to serialize</exception>
        public string Serialize(BehaviorStream<T> tree)
        {
            var serializer = new SBTTOMLSerializer();

            var root = new SBTDictionary();
            root.Add("Size", tree.stream.Length);
            root.Add("GrowBy", tree.GrowBy);
            root.Add("RootId", tree.Root.Value);
            var streamData = root.AddDictionary("StreamData");
            for (var i = 0; i < tree.stream.Length; i++)
            {
                if (tree.stream[i] == null)
                {
                    continue;
                }

                streamData.Add($"{i}_assembly", tree.stream[i].GetType().AssemblyQualifiedName);
                streamData.AddEntry($"{i}_data", tree.stream[i].Serialize());
            }

            serializer.Serialize(root);
            return serializer.GetData();
        }

        /// <summary>
        /// Deserialize the given data into a <see cref="BehaviorStream{T}"/>.
        /// </summary>
        /// <param name="blackboard">the <see cref="IBlackboard"/> the deserialized tree will use</param>
        /// <param name="data">the data to load from</param>
        /// <returns>the deserialized tree</returns>
        /// <exception cref="SerializationException">if the data fails to deserialize</exception>
        public BehaviorStream<T> Deserialize(T blackboard, string data)
        {
            var deserializer = new SBTTOMLDeserializer(data);
            var root = deserializer.GetData<SBTDictionary>();
            int size = root.ReadInt("Size");
            int growBy = root.ReadInt("GrowBy");
            ushort id = root.ReadUShort("RootId");

            BehaviorStream<T> result = new BehaviorStream<T>(blackboard, size, growBy) { Root = new TaskId(id) };

            IDictionary<int, string> typeMap = new Dictionary<int, string>();
            var streamData = root.Read<SBTDictionary>("StreamData");
            for (var i = 0; i < streamData.Count; i++)
            {
                if (!streamData.Contains($"{i}_assembly"))
                {
                    continue;
                }

                string typeName = streamData.ReadString($"{i}_assembly");
                Type type = Type.GetType(typeName);
                if (type == null)
                {
                    throw new SerializationException("Could not get type information for " + typeMap[i]);
                }
                
                typeMap.Add(i, typeName);
                
                Task<T> task = Activator.CreateInstance(type) as Task<T>;
                if (task == null)
                {
                    throw new SerializationException("Could not create task from type " + type.AssemblyQualifiedName);
                }

                task.Deserialize(streamData.Read($"{i}_data"));
                result.stream[i] = task;
            }

            return result;
        }
    }
}
