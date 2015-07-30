using System.Collections.Generic;
using System.Linq;
using KRPC.Service.Attributes;
using KRPC.Utils;

namespace KRPCInfernalRobotics.Services
{
    /// <summary>
    /// A group of servos, obtained by calling <see cref="InfernalRobotics.ServoGroups"/>
    /// or <see cref="InfernalRobotics.ServoGroupWithName"/>. Represents the "Servo Groups"
    /// in the InfernalRobotics UI.
    /// </summary>
    [KRPCClass (Service = "InfernalRobotics")]
    public sealed class ControlGroup : Equatable<ControlGroup>
    {
        readonly IRWrapper.IControlGroup controlGroup;

        internal ControlGroup (IRWrapper.IControlGroup controlGroup)
        {
            this.controlGroup = controlGroup;
        }

        public override bool Equals (ControlGroup obj)
        {
            return controlGroup == obj.controlGroup;
        }

        public override int GetHashCode ()
        {
            return controlGroup.GetHashCode ();
        }

        /// <summary>
        /// The name of the group.
        /// </summary>
        [KRPCProperty]
        public string Name {
            get { return controlGroup.Name; }
            set { controlGroup.Name = value; }
        }

        /// <summary>
        /// The key assigned to be the "forward" key for the group.
        /// </summary>
        [KRPCProperty]
        public string ForwardKey {
            get { return controlGroup.ForwardKey; }
            set { controlGroup.ForwardKey = value; }
        }

        /// <summary>
        /// The key assigned to be the "reverse" key for the group.
        /// </summary>
        [KRPCProperty]
        public string ReverseKey {
            get { return controlGroup.ReverseKey; }
            set { controlGroup.ReverseKey = value; }
        }

        /// <summary>
        /// The speed multiplier for the group.
        /// </summary>
        [KRPCProperty]
        public float Speed {
            get { return controlGroup.Speed; }
            set { controlGroup.Speed = value; }
        }

        /// <summary>
        /// Whether the group is expanded in the InfernalRobotics UI.
        /// </summary>
        [KRPCProperty]
        public bool Expanded {
            get { return controlGroup.Expanded; }
            set { controlGroup.Expanded = value; }
        }

        /// <summary>
        /// The servos that are in the group.
        /// </summary>
        [KRPCProperty]
        public IList<Servo> Servos {
            get { return controlGroup.Servos.Select (x => new Servo (x)).ToList (); }
        }

        /// <summary>
        /// Returns the servo with the given <paramref name="name"/> from this group,
        /// or <c>null</c> if none exists.
        /// </summary>
        /// <param name="name">Name of servo to find.</param>
        [KRPCMethod]
        public Servo ServoWithName (string name)
        {
            var servo = controlGroup.Servos.FirstOrDefault (x => x.Name == name);
            return servo != null ? new Servo (servo) : null;
        }

        /// <summary>
        /// Moves all of the servos in the group to the right.
        /// </summary>
        [KRPCMethod]
        public void MoveRight ()
        {
            controlGroup.MoveRight ();
        }

        /// <summary>
        /// Moves all of the servos in the group to the left.
        /// </summary>
        [KRPCMethod]
        public void MoveLeft ()
        {
            controlGroup.MoveLeft ();
        }

        /// <summary>
        /// Moves all of the servos in the group to the center.
        /// </summary>
        [KRPCMethod]
        public void MoveCenter ()
        {
            controlGroup.MoveCenter ();
        }

        /// <summary>
        /// Moves all of the servos in the group to the next preset.
        /// </summary>
        [KRPCMethod]
        public void MoveNextPreset ()
        {
            controlGroup.MoveNextPreset ();
        }

        /// <summary>
        /// Moves all of the servos in the group to the previous preset.
        /// </summary>
        [KRPCMethod]
        public void MovePrevPreset ()
        {
            controlGroup.MovePrevPreset ();
        }

        /// <summary>
        /// Stops the servos in the group.
        /// </summary>
        [KRPCMethod]
        public void Stop ()
        {
            controlGroup.Stop ();
        }
    }
}
