import React from 'react';
import classes from './OutputCell.module.css';
import * as Styles from '../../../styles/components'
import PropTypes, { InferProps } from "prop-types";

const OutputCell: any = ({...props }: InferProps<typeof OutputCell.propTypes>) => {
    return (
        <label className={classes.output_cell}>
            <Styles.UsualText className={classes.cell} > {props.whatToOutput} </Styles.UsualText>
            <input className={classes.outputt_cell_text} value={props.value} disabled={true} />
        </label>
    );
};

OutputCell.propTypes = {
     props: PropTypes.shape (
        {
            whatToOutput: PropTypes.string.isRequired,
            value: PropTypes.object.isRequired,
        }
    )
};


export default OutputCell;