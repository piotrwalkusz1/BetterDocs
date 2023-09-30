import { SeasonUpdateManyWithoutAnimeItemsInput } from "./SeasonUpdateManyWithoutAnimeItemsInput";

export type AnimeUpdateInput = {
  episodesCount?: number;
  name?: string;
  seasons?: SeasonUpdateManyWithoutAnimeItemsInput;
};
