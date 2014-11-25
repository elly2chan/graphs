namespace AchromaticNum.FullColouringTests

open NUnit.Framework
open FsUnit

let complete (count: int) = 
    

[<TestFixture>]
type ``Main Tests``() = 
    [<Test>]
    member this.``Complete graphs`` () = 
        Assert.GreaterOrEqual
