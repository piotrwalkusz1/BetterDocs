/*
------------------------------------------------------------------------------ 
This code was generated by Amplication. 
 
Changes to this file will be lost if the code is regenerated. 

There are other ways to to customize your code, see this doc to learn more
https://docs.amplication.com/how-to/custom-code

------------------------------------------------------------------------------
  */
import { InputType, Field } from "@nestjs/graphql";
import { ApiProperty } from "@nestjs/swagger";
import { AnimeWhereUniqueInput } from "../../anime/base/AnimeWhereUniqueInput";
import { ValidateNested, IsOptional } from "class-validator";
import { Type } from "class-transformer";
import { StringFilter } from "../../util/StringFilter";

@InputType()
class SeasonWhereInput {
  @ApiProperty({
    required: false,
    type: () => AnimeWhereUniqueInput,
  })
  @ValidateNested()
  @Type(() => AnimeWhereUniqueInput)
  @IsOptional()
  @Field(() => AnimeWhereUniqueInput, {
    nullable: true,
  })
  anime?: AnimeWhereUniqueInput;

  @ApiProperty({
    required: false,
    type: StringFilter,
  })
  @Type(() => StringFilter)
  @IsOptional()
  @Field(() => StringFilter, {
    nullable: true,
  })
  id?: StringFilter;

  @ApiProperty({
    required: false,
    type: StringFilter,
  })
  @Type(() => StringFilter)
  @IsOptional()
  @Field(() => StringFilter, {
    nullable: true,
  })
  name?: StringFilter;
}

export { SeasonWhereInput as SeasonWhereInput };
