# Functional State Management (in C#)

## Terms
* Identity = an entity that has a state.  (In C#, this could be a variable, field, or object)
* State = the value at a point in time. (As of the time of this writing, today's date is 2016-01-25)
* Value = something that doesn't change. (42, 2016-01-25)

These definitions are inspired by http://clojure.org/about/state

## What is state?
"State" - the implicit parameter(s) to a function (everything it needs in order to calculate the function that isn't an explicit argument)

## Status Quo (How is state usually managed in C#)

Let's take an example of the HR department trying to make team members happy by providing their favorite drinks in the kitchen.  On birthdays and service anniversaries, the team members are invited to the kitchen for a free celebratory beverage.

Surveys are given:

* To new hires on their first day.
* Annually (leading up to service anniversaries).
* Following every monthly meeting.

The surveys are processed in batches.

### Bread and Butter (Assignment)
The most idiomatic way of managing state in C# is through assignment.

```csharp
class TeamMember
{
    Drink _favoriteBeverage;
    public Drink FavoriteBeverage => _favoriteBeverage;
    public void UpdateFavorites(TeamSurvey survey)
    {
        _favoriteBeverage = survey.FavoriteDrink;
    }
}
```

### Interlocked Thread Safe
Now, let's say there's a bunch of autonomous agents in your process all processing the survey results.  Also, let's say HR didn't prevent team members from submitting 100 surveys at the same time (to win the X-box drawing).

If you like "lock-free" (and you like to block all your threads at once), you might use Interlocked or Volatile.

```csharp
class TeamMember
{
    Drink _favoriteBeverage;
    public Drink FavoriteBeverage
    {
        get
        {
            Drink d = null;
            Interlocked.Exchange(ref d, _favoriteBeverage);
            return d;
        }    
    }
    public void UpdateFavorites(TeamSurvey survey)
    {
        Interlocked.Exchange(ref _favoriteBeverage, survey.FavoriteFood);
    }
}
```

### Lock Thread Safe
Now, imagine that the requirements change and HR wants to give a complimentary beverage to each team member on birthdays and service anniversaries, but only if the kitchen stocks the beverage.

So now that we need a flag to indicate whether the kitchen keeps the beverage in stock, interlocked may be a bad idea since related variables are not updated atomically.  You can imagine a race condition when a team member submits 50 surveys for Mountain Dew and 50 for some fine cabernet.  One is usually stocked, and the other isn't.  The team member might be in for a let-down when the congratulatory email says the wine is available.

```csharp
class TeamMember
{
    Drink _favoriteBeverage;
    bool _favoriteBeverageIsStocked;
    public Drink FavoriteBeverage
    {
        get
        {
            lock(_lock)
            {
                return _favoriteFood;
            }
        }    
    }
    public void UpdateFavorites(TeamSurvey survey, Func<Drink,bool> drinkIsStocked)
    {
        lock(_lock)
        {
            _favoriteFood = survey.FavoriteFood;
            _favoriteBeverageIsStocked = drinkIsStocked(_favoriteBeverage);
        }
    }
}
```

### Summary of the idiomatic C#
We observe in the Favorite Beverages example, that the familiar/idiomatic ways of managing state are tricky.  The field being changed needed to be protected by either `Interlocked` or `lock/Monitor` and this led to more verbose code, with state-transitions that are more difficult to test.

Once we start needing to "protect" variables, the code starts getting more verbose and complicated.  It becomes difficult to reason about what is going on.  So in a multi-threaded environment, what do we need to protected?  Only variables that change (mutate) - things that are assigned.

## Constraints 
Cristina Lopes and Uncle Bob have similar things to say about programming style being about constraints.

* [Cristina Lopes discusses the idea of using constraints to define styles of programming and architecture](http://www.infoq.com/presentations/style-methodology).  I saw her speak at StrangeLoop and I loved the way she implemented a bunch of different programming styles (but all in Python).  Here's her book on the subject: [Excercises in Programming Style](http://www.amazon.com/Exercises-Programming-Style-Cristina-Videira/dp/1482227371).
* Uncle Bob describes the constraints that define [Three Paradigms](https://blog.8thlight.com/uncle-bob/2012/12/19/Three-Paradigms.html) of programming (Structured, Object-Oriented, Functional). For example, "All functional programs are dominated by one huge constraint. They don’t use assignment."

### Constraining Assignment
Since assignment makes things harder to reason about and harder to test, perhaps we should avoid it.  But without assignment, how can we change state?

So if we can't eliminate assignment, can we manage it by using a more predictable idiom?

## Atoms
Another way to think about state management is Atoms.  There are variants of AtomicReferences in [Java8](https://docs.oracle.com/javase/8/docs/api/?java/util/concurrent/atomic/AtomicReference.html), [Akka.net] (http://api.getakka.net/docs/stable/html/A8A879A7.htm), [Clojure-Clr](https://github.com/clojure/clojure-clr/blob/master/Clojure/Clojure/Lib/AtomicReference.cs), [C++11](http://en.cppreference.com/w/cpp/atomic), and many more.

I was first introduced to atoms through Clojure/ClojureScript.

In object-oriented languages, Atoms are objects that hold state and only change it through a specific set of operations.  For example, an atom usually supports these operations:

* Reset - ignore the value that was there before and set the atom to a new value.
  * In C#, `myAge.Update(36)`.
* Update - take the old value and use it in the calculation of a new value for the atom.
  * In C#, `myAge.Update(old => old + 1)`.
* GetValue - get the current value thread-safely
  * In C#, `Console.WriteLine(myAge.Value)`

Remember, "value" means something that doesn't change.

Some atom implementations support validators or guards that prevent the atom from violating an assertion.  Here is clojure-clr's implementation of Update() (or "`swap`" in [clojure-clr](https://github.com/clojure/clojure-clr/blob/5beaf1162ac853795f88bba977b13ba35c1416c5/Clojure/Clojure/Lib/Atom.cs#L80)).

This method takes in a (Clojure) function used to calculate the new value, validates the value, and then notifies any watchers of the new value.

```csharp
        public object swap(IFn f)
        {
            for (; ; )
            {
                object v = deref();
                object newv = f.invoke(v);
                Validate(newv);
                if (_state.CompareAndSet(v, newv))
                {
                    NotifyWatches(v,newv);
                    return newv;
                }
            }
        }
```

Since it uses compare and set, if another thread updated the value between `deref()` and `CompareAndSet`, it may have to loop and recalculate the replacement value as a function of the atom's "new" current value.

### Favorite Drink Survey with Atoms

```csharp
immutable class TeamMember
{
    public Drink FavoriteBeverage;
    public bool FavoriteBeverageIsStocked;
}

class SurveyProcessor
{
    Func<Drink,bool> drinkIsStocked;
    public void UpdateFavorites(Atom<TeamMember> teamMember, TeamSurvey survey)
    {
        teamMember.Update(tm => WithFavoriteDrink(tm, survey.FavoriteDrink));
    }

    internal static TeamMember WithFavoriteDrink(TeamMember teamMember, Drink favorite, Func<Drink,bool> drinkIsStocked) =>
        teamMember
            .WithFavoriteDrink(favorite)
            .WithFavoriteBeverageIsStocked(drinkIsStocked(favorite));
}
```

Wait!  What? C# supports Immutable classes?

Nope, sorry, C# isn't that cool yet.  I'm just trying to paint a picture.

### Immutbility
Today, there isn't a great for immutability in C#.  We have to [roll our own](https://github.com/jeremyrsellars/Valuable/blob/b4f04dbb2a4aefd5dc356dc9ebe30d66f06b61ba/ConsoleApplication1/MailingAddress.cs#L9) immutable "values."  I hope that will change soon.

Today:
* [System.Collections.Immutable](https://msdn.microsoft.com/en-us/library/system.collections.immutable(v=vs.111).aspx)
* Structs with `MemberwiseClone`
