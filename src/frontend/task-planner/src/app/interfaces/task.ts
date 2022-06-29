import { EStatus } from "../enums/status";

export interface ITask {
    id: string | undefined;
    name: string;
    description: string;
    plannedEndDate: Date;
    startDate: Date;
    endDate: Date;
    status: EStatus;
}