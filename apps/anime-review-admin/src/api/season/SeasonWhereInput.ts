import { AnimeWhereUniqueInput } from "../anime/AnimeWhereUniqueInput";
import { StringFilter } from "../../util/StringFilter";

export type SeasonWhereInput = {
  anime?: AnimeWhereUniqueInput;
  id?: StringFilter;
  name?: StringFilter;
};
