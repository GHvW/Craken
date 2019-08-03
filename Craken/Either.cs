using System;
using System.Collections.Generic;
using System.Text;

namespace Craken {

    public struct Unit { }

    public abstract class Either<Left, Right> { }

    public class Left<A> : Either<A, Unit> {
        private A data;

        public Left(A data) {
            this.data = data;
        }
    }

    public class Right<A> : Either<Unit, A> {

        private A data;

        public Right(A data) {
            this.data = data;
        }
    }

    public static class EitherExtensions {
        
        public static Either<A, C> Map<A, B, C>(this Either<A, B> data, Func<B, C> transform) {
            return data switch {

            }
        }
    }
}
