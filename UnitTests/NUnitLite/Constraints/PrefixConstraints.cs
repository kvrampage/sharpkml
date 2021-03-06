// ***********************************************************************
// Copyright (c) 2007 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections;

namespace NUnit.Framework.Constraints
{
	#region PrefixConstraint
	/// <summary>
	/// Abstract base class used for prefixes
	/// </summary>
    public abstract class PrefixConstraint : Constraint
    {
        /// <summary>
        /// The base constraint
        /// </summary>
        protected Constraint baseConstraint;

        /// <summary>
        /// Construct given a base constraint
        /// </summary>
        /// <param name="resolvable"></param>
        protected PrefixConstraint(IResolveConstraint resolvable) : base(resolvable)
        {
            if ( resolvable != null )
                this.baseConstraint = resolvable.Resolve();
        }
    }
	#endregion

	#region NotConstraint
    /// <summary>
	/// NotConstraint negates the effect of some other constraint
	/// </summary>
	public class NotConstraint : PrefixConstraint
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:NotConstraint"/> class.
		/// </summary>
		/// <param name="baseConstraint">The base constraint to be negated.</param>
		public NotConstraint(Constraint baseConstraint)
			: base( baseConstraint ) { }

		/// <summary>
		/// Test whether the constraint is satisfied by a given value
		/// </summary>
		/// <param name="actual">The value to be tested</param>
		/// <returns>True for if the base constraint fails, false if it succeeds</returns>
		public override bool Matches(object actual)
		{
			this.actual = actual;
			return !baseConstraint.Matches(actual);
		}

		/// <summary>
		/// Write the constraint description to a MessageWriter
		/// </summary>
		/// <param name="writer">The writer on which the description is displayed</param>
		public override void WriteDescriptionTo( MessageWriter writer )
		{
			writer.WritePredicate( "not" );
			baseConstraint.WriteDescriptionTo( writer );
		}

		/// <summary>
		/// Write the actual value for a failing constraint test to a MessageWriter.
		/// </summary>
		/// <param name="writer">The writer on which the actual value is displayed</param>
		public override void WriteActualValueTo(MessageWriter writer)
		{
			baseConstraint.WriteActualValueTo (writer);
		}
	}
	#endregion

	#region AllItemsConstraint
	/// <summary>
	/// AllItemsConstraint applies another constraint to each
	/// item in a collection, succeeding if they all succeed.
	/// </summary>
	public class AllItemsConstraint : PrefixConstraint
	{
		/// <summary>
		/// Construct an AllItemsConstraint on top of an existing constraint
		/// </summary>
		/// <param name="itemConstraint"></param>
		public AllItemsConstraint(Constraint itemConstraint)
			: base( itemConstraint )
        {
            this.DisplayName = "all";
        }

		/// <summary>
		/// Apply the item constraint to each item in the collection,
		/// failing if any item fails.
		/// </summary>
		/// <param name="actual"></param>
		/// <returns></returns>
		public override bool Matches(object actual)
		{
			this.actual = actual;

			if ( !(actual is IEnumerable) )
				throw new ArgumentException( "The actual value must be an IEnumerable", "actual" );

			foreach(object item in (IEnumerable)actual)
				if (!baseConstraint.Matches(item))
					return false;

			return true;
		}

		/// <summary>
		/// Write a description of this constraint to a MessageWriter
		/// </summary>
		/// <param name="writer"></param>
		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.WritePredicate("all items");
			baseConstraint.WriteDescriptionTo(writer);
		}
	}
	#endregion

	#region SomeItemsConstraint
	/// <summary>
	/// SomeItemsConstraint applies another constraint to each
	/// item in a collection, succeeding if any of them succeeds.
	/// </summary>
	public class SomeItemsConstraint : PrefixConstraint
	{
		/// <summary>
		/// Construct a SomeItemsConstraint on top of an existing constraint
		/// </summary>
		/// <param name="itemConstraint"></param>
		public SomeItemsConstraint(Constraint itemConstraint)
			: base( itemConstraint ) 
        {
            this.DisplayName = "some";
        }

		/// <summary>
		/// Apply the item constraint to each item in the collection,
		/// succeeding if any item succeeds.
		/// </summary>
		/// <param name="actual"></param>
		/// <returns></returns>
		public override bool Matches(object actual)
		{
			this.actual = actual;

			if ( !(actual is IEnumerable) )
				throw new ArgumentException( "The actual value must be an IEnumerable", "actual" );

			foreach(object item in (IEnumerable)actual)
				if (baseConstraint.Matches(item))
					return true;

			return false;
		}

		/// <summary>
		/// Write a description of this constraint to a MessageWriter
		/// </summary>
		/// <param name="writer"></param>
		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.WritePredicate("some item");
			baseConstraint.WriteDescriptionTo(writer);
		}
	}
	#endregion

	#region NoItemConstraint
    /// <summary>
    /// NoItemConstraint applies another constraint to each
    /// item in a collection, failing if any of them succeeds.
    /// </summary>
    public class NoItemConstraint : PrefixConstraint
	{
		/// <summary>
		/// Construct a SomeItemsConstraint on top of an existing constraint
		/// </summary>
		/// <param name="itemConstraint"></param>
		public NoItemConstraint(Constraint itemConstraint)
			: base( itemConstraint ) 
        {
            this.DisplayName = "none";
        }

		/// <summary>
		/// Apply the item constraint to each item in the collection,
		/// failing if any item fails.
		/// </summary>
		/// <param name="actual"></param>
		/// <returns></returns>
		public override bool Matches(object actual)
		{
			this.actual = actual;

			if ( !(actual is IEnumerable) )
				throw new ArgumentException( "The actual value must be an IEnumerable", "actual" );

			foreach(object item in (IEnumerable)actual)
				if (baseConstraint.Matches(item))
					return false;

			return true;
		}

		/// <summary>
		/// Write a description of this constraint to a MessageWriter
		/// </summary>
		/// <param name="writer"></param>
		public override void WriteDescriptionTo(MessageWriter writer)
		{
			writer.WritePredicate("no item");
			baseConstraint.WriteDescriptionTo(writer);
		}
	}
	#endregion
}