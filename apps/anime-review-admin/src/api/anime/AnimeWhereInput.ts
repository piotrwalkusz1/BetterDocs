import { StringFilter } from "../../util/StringFilter";
import { SeasonListRelationFilter } from "../season/SeasonListRelationFilter";

export type AnimeWhereInput = {
  id?: StringFilter;
  name?: StringFilter;
  seasons?: SeasonListRelationFilter;
};
