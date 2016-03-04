module RuleNormalizer2

open Common
open RuleParser2



type NormalizedRule =
  {
    Input : List<Id>
    Output : Id
    Premis : List<Id>
    Typemap : List<Id>
    
  }

