module IR
open Common
open ParserMonad
(*
let nop:Parser<byte,_,unit> =
  prs{
    do! 
  }
    

type prefix_instruction = 
  | constrained    = 0xFE16
  | no             = 0xFE19
  | readonly       = 0xFE1E
  | tail           = 0xFE14
  | unaligned      = 0xFE12
  | volatile_      = 0xFE13

type base_instruction = 
  | nop            = 0x00 
  | break_         = 0x01 
  | ldarg_0        = 0x02 
  | ldarg_1        = 0x03 
  | ldarg_2        = 0x04 
  | ldarg_3        = 0x05 
  | ldloc_0        = 0x06 
  | ldloc_1        = 0x07 
  | ldloc_2        = 0x08 
  | ldloc_3        = 0x09 
  | stloc_0        = 0x0A 
  | stloc_1        = 0x0B 
  | stloc_2        = 0x0C 
  | stloc_3        = 0x0D 
  | ldarg_s        = 0x0E 
  | ldarga_s       = 0x0F 
  | starg_s        = 0x10 
  | ldloc_s        = 0x11 
  | ldloca_s       = 0x12 
  | stloc_s        = 0x13 
  | ldnull         = 0x14 
  | ldc_i4_m1      = 0x15 
  | ldc_i4_0       = 0x16 
  | ldc_i4_1       = 0x17 
  | ldc_i4_2       = 0x18 
  | ldc_i4_3       = 0x19 
  | ldc_i4_4       = 0x1A 
  | ldc_i4_5       = 0x1B 
  | ldc_i4_6       = 0x1C 
  | ldc_i4_7       = 0x1D 
  | ldc_i4_8       = 0x1E 
  | ldc_i4_s       = 0x1F 
  | ldc_i4         = 0x20 
  | ldc_i8         = 0x21 
  | ldc_r4         = 0x22 
  | ldc_r8         = 0x23 
  | dup            = 0x25 
  | pop            = 0x26 
  | jmp            = 0x27 
  | call           = 0x28 
  | calli          = 0x29 
  | ret            = 0x2A 
  | br_s           = 0x2B 
  | brfalse_s      = 0x2C 
  | brtrue_s       = 0x2D 
  | beq_s          = 0x2E 
  | bge_s          = 0x2F 
  | bgt_s          = 0x30 
  | ble_s          = 0x31 
  | blt_s          = 0x32 
  | bne_un_s       = 0x33 
  | bge_un_s       = 0x34 
  | bgt_un_s       = 0x35 
  | ble_un_s       = 0x36 
  | blt_un_s       = 0x37 
  | br             = 0x38 
  | brfalse        = 0x39 
  | brtrue         = 0x3A 
  | beq            = 0x3B 
  | bge            = 0x3C 
  | bgt            = 0x3D 
  | ble            = 0x3E 
  | blt            = 0x3F 
  | bne_un         = 0x40 
  | bge_un         = 0x41 
  | bgt_un         = 0x42 
  | ble_un         = 0x43 
  | blt_un         = 0x44 
  | switch         = 0x45 
  | ldind_i1       = 0x46 
  | ldind_u1       = 0x47 
  | ldind_i2       = 0x48 
  | ldind_u2       = 0x49 
  | ldind_i4       = 0x4A 
  | ldind_u4       = 0x4B 
  | ldind_i8       = 0x4C 
  | ldind_i        = 0x4D 
  | ldind_r4       = 0x4E 
  | ldind_r8       = 0x4F 
  | ldind_ref      = 0x50 
  | stind_ref      = 0x51 
  | stind_i1       = 0x52 
  | stind_i2       = 0x53 
  | stind_i4       = 0x54 
  | stind_i8       = 0x55 
  | stind_r4       = 0x56 
  | stind_r8       = 0x57 
  | add            = 0x58 
  | sub            = 0x59 
  | mul            = 0x5A 
  | div            = 0x5B 
  | div_un         = 0x5C 
  | rem            = 0x5D 
  | rem_un         = 0x5E 
  | and_           = 0x5F 
  | or_            = 0x60 
  | xor            = 0x61 
  | shl            = 0x62 
  | shr            = 0x63 
  | shr_un         = 0x64 
  | neg            = 0x65 
  | not            = 0x66 
  | conv_i1        = 0x67 
  | conv_i2        = 0x68 
  | conv_i4        = 0x69 
  | conv_i8        = 0x6A 
  | conv_r4        = 0x6B 
  | conv_r8        = 0x6C 
  | conv_u4        = 0x6D 
  | conv_u8        = 0x6E 
  | callvirt       = 0x6F 
  | cpobj          = 0x70 
  | ldobj          = 0x71 
  | ldstr          = 0x72 
  | newobj         = 0x73 
  | castclass      = 0x74 
  | isinst         = 0x75 
  | conv_r_un      = 0x76 
  | unbox          = 0x79 
  | throw          = 0x7A 
  | ldfld          = 0x7B 
  | ldflda         = 0x7C 
  | stfld          = 0x7D 
  | ldsfld         = 0x7E 
  | ldsflda        = 0x7F 
  | stsfld         = 0x80 
  | stobj          = 0x81 
  | conv_ovf_i1_un = 0x82 
  | conv_ovf_i2_un = 0x83 
  | conv_ovf_i4_un = 0x84 
  | conv_ovf_i8_un = 0x85 
  | conv_ovf_u1_un = 0x86 
  | conv_ovf_u2_un = 0x87 
  | conv_ovf_u4_un = 0x88 
  | conv_ovf_u8_un = 0x89 
  | conv_ovf_i_un  = 0x8A 
  | conv_ovf_u_un  = 0x8B 
  | box            = 0x8C 
  | newarr         = 0x8D 
  | ldlen          = 0x8E 
  | ldelema        = 0x8F 
  | ldelem_i1      = 0x90 
  | ldelem_u1      = 0x91 
  | ldelem_i2      = 0x92 
  | ldelem_u2      = 0x93 
  | ldelem_i4      = 0x94 
  | ldelem_u4      = 0x95 
  | ldelem_i8      = 0x96 
  | ldelem_i       = 0x97 
  | ldelem_r4      = 0x98 
  | ldelem_r8      = 0x99 
  | ldelem_ref     = 0x9A 
  | stelem_i       = 0x9B 
  | stelem_i1      = 0x9C 
  | stelem_i2      = 0x9D 
  | stelem_i4      = 0x9E 
  | stelem_i8      = 0x9F 
  | stelem_r4      = 0xA0 
  | stelem_r8      = 0xA1 
  | stelem_ref     = 0xA2 
  | ldelem         = 0xA3 
  | stelem         = 0xA4 
  | unbox_any      = 0xA5 
  | conv_ovf_i1    = 0xB3 
  | conv_ovf_u1    = 0xB4 
  | conv_ovf_i2    = 0xB5 
  | conv_ovf_u2    = 0xB6 
  | conv_ovf_i4    = 0xB7 
  | conv_ovf_u4    = 0xB8 
  | conv_ovf_i8    = 0xB9 
  | conv_ovf_u8    = 0xBA 
  | refanyval      = 0xC2 
  | ckfinite       = 0xC3 
  | mkrefany       = 0xC6 
  | ldtoken        = 0xD0 
  | conv_u2        = 0xD1 
  | conv_u1        = 0xD2 
  | conv_i         = 0xD3 
  | conv_ovf_i     = 0xD4 
  | conv_ovf_u     = 0xD5 
  | add_ovf        = 0xD6 
  | add_ovf_un     = 0xD7 
  | mul_ovf        = 0xD8 
  | mul_ovf_un     = 0xD9 
  | sub_ovf        = 0xDA 
  | sub_ovf_un     = 0xDB 
  | endfinally     = 0xDC 
  | leave          = 0xDD 
  | leave_s        = 0xDE 
  | stind_i        = 0xDF 
  | conv_u         = 0xE0 
  | arglist        = 0xFE00 
  | ceq            = 0xFE01 
  | cgt            = 0xFE02 
  | cgt_un         = 0xFE03 
  | clt            = 0xFE04 
  | clt_un         = 0xFE05 
  | ldftn          = 0xFE06 
  | ldvirtftn      = 0xFE07 
  | ldarg          = 0xFE09 
  | ldarga         = 0xFE0A 
  | starg          = 0xFE0B 
  | ldloc          = 0xFE0C 
  | ldloca         = 0xFE0D 
  | stloc          = 0xFE0E 
  | localloc       = 0xFE0F 
  | endfilter      = 0xFE11 
  | Initobj        = 0xFE15 
  | cpblk          = 0xFE17 
  | initblk        = 0xFE18 
  | rethrow        = 0xFE1A 
  | sizeof         = 0xFE1C 
  | Refanytype     = 0xFE1D 

type Instruction = Base   of base_instruction
                 | Prefix of prefix_instruction*Instruction

type IR = Assignment         of Id*TypedValue
        | Call               of Id*Type*Id*List<TypedValue>
        | CallWithSideEffect of Id*Type*Id*List<TypedValue>
        | Foreach            of Type*BasicBlock

and TypedValue = Type*Value
and Value = Identifier    of Id
          | IntLiteral    of System.Int64
          | FloatLiteral  of System.Double
          | StringLiteral of System.String

and BasicBlock = List<IR>
and Function   = List<Id*Type>*Map<Id,BasicBlock>

(*
and BinOp = Add of IntType*IntType | FAdd of FloatType*FloatType
          | Sub of IntType*IntType | FSub of FloatType*FloatType
          | Mul of IntType*IntType | FMul of FloatType*FloatType
          | Div of IntType*IntType | FDiv of FloatType*FloatType | UDiv of IntType*IntType
          | Mod of IntType*IntType | FMod of FloatType*FloatType | UMod of IntType*IntType
          | Rem of IntType*IntType | FRem of FloatType*FloatType | URem of IntType*IntType
          | Shl of IntType*IntType | LShr of IntType*IntType     | AShr of IntType*IntType
          | And of IntType*IntType | BAnd
          | Or  of IntType*IntType | BOr
          | Xor of IntType*IntType | BXor
*)
          
and Type = Seq   of Type 
         | Tuple of Type*Type
         | Union of Type*Type
         | Int   of IntType
         | Float of FloatType
         | Bool | String | Label

and IntType   = Byte | Short | Int | Long
and FloatType = Single | Double
*)