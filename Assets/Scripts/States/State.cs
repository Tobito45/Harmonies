using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IState
{
    public void Entry();
    public void Exit();
}
