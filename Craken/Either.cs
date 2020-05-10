using System;
using System.Collections.Generic;
using System.Text;

// COPY PASTE FROM GHvW/Heresy/EitherStatic
namespace Craken {

    public interface IEither<A, B> {

        public IEither<A, Result> Map<Result>(Func<B, Result> transform);

        public IEither<Result, B> MapLeft<Result>(Func<A, Result> transform);

        public IEither<A, Result> Bind<Result>(Func<B, IEither<A, Result>> transform);

        public bool IsLeft(); // make get?

        public bool IsRight(); // make get?
    }

    public static class Either<A, B> {

        private class _Left : IEither<A, B> {

            private readonly A data;

            public _Left(A data) {
                this.data = data;
            }

            public IEither<A, Result> Bind<Result>(Func<B, IEither<A, Result>> transform) {
                throw new NotImplementedException();
            }

            public bool IsLeft() => true;

            public bool IsRight() => false; 

            public IEither<A, Result> Map<Result>(Func<B, Result> transform) => (IEither<A, Result>) this;

            public IEither<Result, B> MapLeft<Result>(Func<A, Result> transform) => Either<Result, B>.Left(transform(this.data));
        }

        private class _Right : IEither<A, B> {

            private readonly B data;

            public _Right(B data) {
                this.data = data;
            }

            public IEither<A, Result> Bind<Result>(Func<B, IEither<A, Result>> transform) {
                throw new NotImplementedException();
            }

            public bool IsLeft() => false;

            public bool IsRight() => true;

            public IEither<A, Result> Map<Result>(Func<B, Result> transform) => Either<A, Result>.Right(transform(this.data));

            public IEither<Result, B> MapLeft<Result>(Func<A, Result> transform) => (IEither<Result, B>) this;
        }

        public static IEither<A, B> Left(A data) => new _Left(data);

        public static IEither<A, B> Right(B data) => new _Right(data);
    }

    //public struct Unit { }

    //public interface Either<Left, Right> {
    //    Either<Left, Result> Map<Result>(Func<Right, Result> transform);
    //    Either<Result, Right> MapLeft<Result>(Func<Left, Result> transform);
    //}

    //public class Right<B> : Either<Unit, B> {

    //    private readonly B Data;
    //    public Right(B data) {
    //        this.Data = data;
    //    }

    //    public Either<Unit, Result> Map<Result>(Func<B, Result> transform) => new Right<Result>(transform(this.Data));

    //    public Either<Result, B> MapLeft<Result>(Func<A, Result> transform) => (Either<Result, B>) this;
    //}

    //public class Left<A> : Either<A, B> {

    //    public readonly A Data;

    //    public Left(A data) {
    //        this.Data = data;
    //    }

    //    public Either<A, Result> Map<Result>(Func<B, Result> transform) => (Either<A, Result>) this;

    //    public Either<Result, B> MapLeft<Result>(Func<A, Result> transform) {
    //        throw new NotImplementedException();
    //    }
    //}
}
