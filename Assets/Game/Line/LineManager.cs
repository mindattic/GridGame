using System.Linq;
using UnityEngine;

public class LineManager : ExtendedMonoBehavior
{
    private void Start()
    {
    
    }

    private void Update()
    {
       
    }

    public void Reset()
    {
        lines.ForEach(x => x.Hide());
    }

}
