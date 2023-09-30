import { SortOrder } from "../../util/SortOrder";

export type AnimeOrderByInput = {
  createdAt?: SortOrder;
  episodesCount?: SortOrder;
  id?: SortOrder;
  name?: SortOrder;
  updatedAt?: SortOrder;
};
