import { AnimeWhereUniqueInput } from "../anime/AnimeWhereUniqueInput";

export type SeasonCreateInput = {
  anime?: AnimeWhereUniqueInput | null;
  name: string;
};
