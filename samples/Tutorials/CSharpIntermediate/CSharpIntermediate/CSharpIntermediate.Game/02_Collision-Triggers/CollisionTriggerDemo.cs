// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
using System.Collections.Specialized;
using Stride.Core.Collections;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Physics;

namespace CSharpIntermediate.Code
{
    public class CollisionTriggerDemo : SyncScript
    {
        PhysicsComponent triggerCollider;
        string collisionStatus = "";

        public override void Start()
        {
            // Retrieve the Physics component of the current entity
            triggerCollider = Entity.Get<PhysicsComponent>();

            // When the 'CollectionChanged' event occurs, execute the CollisionsChanged method
            triggerCollider.Collisions.CollectionChanged += CollisionsChanged;
        }

        private void CollisionsChanged(object sender, TrackingCollectionChangedEventArgs args)
        {
            // Cast the argument 'item' to a collision object
            var collision = (Collision)args.Item;

            // We need to make sure which collision object is not the Trigger collider
            // We perform a little check to find the ballCollider 
            var ballCollider = triggerCollider == collision.ColliderA ? collision.ColliderB : collision.ColliderA;

            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                // When a collision has been added to the collision collection, we know an object has 'entered' our trigger
                collisionStatus = ballCollider.Entity.Name + " entered " + triggerCollider.Entity.Name;
            }
            else if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                // When a collision has been removed from the collision collection, we know an object 'left' our trigger
                collisionStatus = ballCollider.Entity.Name + " left " + triggerCollider.Entity.Name;
            }
        }

        public override void Update()
        {
            // The trigger collider can have 0, 1, or multiple collision going on in a single frame
            var drawY = 280;
            foreach (var collision in triggerCollider.Collisions)
            {
                DebugText.Print("ColliderA: " + collision.ColliderA.Entity.Name, new Int2(500, drawY += 20));
                DebugText.Print("ColliderB: " + collision.ColliderB.Entity.Name, new Int2(500, drawY += 20));
            }

            DebugText.Print(collisionStatus, new Int2(500, 400));
        }
    }
}
