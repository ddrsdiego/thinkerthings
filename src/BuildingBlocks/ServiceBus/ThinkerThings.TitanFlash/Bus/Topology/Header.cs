using System.Collections.Generic;
using System.Linq;

namespace ThinkerThings.TitanFlash.Bus.Topology
{
    public class Header
    {
        private Dictionary<string, object> _values;

        public static Header Default() => new Header();

        protected Header()
        {
            _values = null;
        }

        public Dictionary<string, object> Values
        {
            get
            {
                return _values?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
        }

        public void Add(string key, object value)
        {
            if(_values == null)
            {
                _values = new Dictionary<string, object>();
            }

            _values.Add(key, value);
        }
    }
}
