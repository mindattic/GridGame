using System.Collections;
using System.Linq;

namespace Game.Manager
{
    public class ActorManager : ExtendedMonoBehavior
    {

        public void Clear()
        {
            actors.ForEach(x => Destroy(x.gameObject));
            actors.Clear();
        }
    }
}