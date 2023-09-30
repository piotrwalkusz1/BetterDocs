import { SeasonCreateNestedManyWithoutAnimeItemsInput } from "./SeasonCreateNestedManyWithoutAnimeItemsInput";

export type AnimeCreateInput = {
  episodesCount: number;
  name: string;
  seasons?: SeasonCreateNestedManyWithoutAnimeItemsInput;
};
