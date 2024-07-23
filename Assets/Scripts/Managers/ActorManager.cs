using System.Collections;
using System.Linq;

public class ActorManager : ExtendedMonoBehavior
{

    public void Clear()
    {
        actors.ForEach(x => Destroy(x.gameObject));
        actors.Clear();
    }


}
