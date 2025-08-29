using System;
using System.Collections.Generic;

using F10Y.T0003;


namespace F10Y.L0030
{
    [ValuesMarker]
    public partial interface IKindMarkerSets
    {
        /// <summary>
        /// All kind markers.
        /// </summary>
        public char[] All =>
        [
            Instances.KindMarkers._ExclamationPoint,
            Instances.KindMarkers._E,
            Instances.KindMarkers._F,
            Instances.KindMarkers._M,
            Instances.KindMarkers._N,
            Instances.KindMarkers._P,
            Instances.KindMarkers._T
        ];

        /// <inheritdoc cref="All"/>
        public char[] All_AsArray => this.All;

        private static readonly Lazy<HashSet<char>> zAll_Hash = new(() =>
        [
            IKindMarkers.Error_Constant,
            IKindMarkers.Event_Constant,
            IKindMarkers.Field_Constant,
            IKindMarkers.Method_Constant,
            IKindMarkers.Namespace_Constant,
            IKindMarkers.Property_Constant,
            IKindMarkers.Type_Constant,
        ]);

        public HashSet<char> All_AsHash => zAll_Hash.Value;
    }
}
