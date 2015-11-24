module CodeGen

let mangle (name:string) :string =
  let normalize (name:string) :string =
    let bytes = System.Text.Encoding.Default.GetBytes(name)
    let str   = System.Text.Encoding.UTF8.GetString(bytes)
    System.String.Intern(str).Normalize(System.Text.NormalizationForm.FormKC)
  let mangleChar c =
    if (c>='a'&&c<='z') || (c>='A'&&c<='Z') || (c>='0'&&c<='9')
    then sprintf "%c" c
    else sprintf "_%02X" (System.Convert.ToByte(c))
  name |> normalize |> String.collect mangleChar
