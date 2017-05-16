using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace HG
{
	public class MainThreadInvoke : Singleton<MainThreadInvoke> 
	{
		protected MainThreadInvoke () {} 

		private Queue<Action> _actions = new Queue<Action>();

		public void Add(Action action)
		{
			lock(_actions)
			{
				_actions.Enqueue(action);
			}
		}

		void Update()
		{
			lock(_actions)
			{
				while(_actions.Count>0)
				{
					var action = _actions.Dequeue();
					action();
				}
			}
		}
	}
}