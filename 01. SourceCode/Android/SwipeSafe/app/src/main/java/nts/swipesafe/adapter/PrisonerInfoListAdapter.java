package nts.swipesafe.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.R;
import nts.swipesafe.common.DateUtils;
import nts.swipesafe.common.Utils;
import nts.swipesafe.model.PrisonerInfoModel;
import nts.swipesafe.model.PrisonerModel;

/**
 * {@link RecyclerView.Adapter}
 * TODO: Replace the implementation with code for your data type.
 */
public class PrisonerInfoListAdapter extends RecyclerView.Adapter<PrisonerInfoListAdapter.ViewHolder> {

    private final int mBackground;
    private List<PrisonerInfoModel> listObject = new ArrayList<>();

    private final TypedValue mTypedValue = new TypedValue();

    private Context ctx;
    private OnItemClickListener mOnItemClickListener;

    public interface OnItemClickListener {
        void onDeleteClick(View view, int position, PrisonerInfoModel obj);
    }

    public void SetOnItemClickListener(final OnItemClickListener mItemClickListener) {
        this.mOnItemClickListener = mItemClickListener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        // each data item is just a string in this case
        public LinearLayout lyName, lyGender, lyBirthday, lyAge, lyAddress, lyRelationship, lyDelete;
        public TextView txtName, txtGender, txtBirthday, txtAge, txtAddress, txtRelationship;
        public RecyclerView rvAbuse;
        public TextView txtTitle;

        public ViewHolder(View v) {
            super(v);
            lyName = v.findViewById(R.id.lyName);
            lyGender = v.findViewById(R.id.lyGender);
            lyBirthday = v.findViewById(R.id.lyBirthday);
            lyAge = v.findViewById(R.id.lyAge);
            lyAddress = v.findViewById(R.id.lyAddress);
            lyRelationship = v.findViewById(R.id.lyRelationship);

            txtName = v.findViewById(R.id.txtName);
            txtGender = v.findViewById(R.id.txtGender);
            txtBirthday = v.findViewById(R.id.txtBirthday);
            txtAge = v.findViewById(R.id.txtAge);
            txtRelationship = v.findViewById(R.id.txtRelationship);
            txtAddress = v.findViewById(R.id.txtAddress);

            rvAbuse = v.findViewById(R.id.rvAbuse);

            lyDelete = v.findViewById(R.id.lyDelete);
            txtTitle = v.findViewById(R.id.txtTitle);
        }
    }

    // Provide a suitable constructor (depends on the kind of dataset)
    public PrisonerInfoListAdapter(Context ctx, List<PrisonerInfoModel> items) {
        this.ctx = ctx;
        listObject = items != null ? items : new ArrayList<PrisonerInfoModel>();
        ctx.getTheme().resolveAttribute(R.attr.selectableItemBackground, mTypedValue, true);
        mBackground = mTypedValue.resourceId;
    }

    @Override
    public PrisonerInfoListAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_prisoner_detail, parent, false);
        v.setBackgroundResource(mBackground);
        // set the view's size, margins, paddings and layout parameters
        ViewHolder vh = new ViewHolder(v);
        return vh;
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(ViewHolder holder, final int position) {
        final PrisonerInfoModel prisonerModel = listObject.get(position);

        holder.txtTitle.setText("Đối tượng #" + (position + 1));

        if (!Utils.isEmpty(prisonerModel.name)) {
            holder.lyName.setVisibility(View.VISIBLE);
            holder.txtName.setText(prisonerModel.name);
        } else {
            holder.lyName.setVisibility(View.GONE);
        }

        if (!Utils.isEmpty(prisonerModel.gender)) {
            holder.lyGender.setVisibility(View.VISIBLE);
            holder.txtGender.setText(prisonerModel.gender);
        } else {
            holder.lyGender.setVisibility(View.GONE);
        }

        if (!Utils.isEmpty(prisonerModel.birthday)) {
            holder.lyBirthday.setVisibility(View.VISIBLE);
            holder.txtBirthday.setText(prisonerModel.birthday);
        } else {
            holder.lyBirthday.setVisibility(View.GONE);
        }

        if (!Utils.isEmpty(prisonerModel.age)) {
            holder.lyAge.setVisibility(View.VISIBLE);
            holder.txtAge.setText(String.valueOf(prisonerModel.age));
        } else {
            holder.lyAge.setVisibility(View.GONE);
        }

        if (!Utils.isEmpty(prisonerModel.relationship)) {
            holder.lyRelationship.setVisibility(View.VISIBLE);
            holder.txtRelationship.setText(String.valueOf(prisonerModel.relationship));
        } else {
            holder.lyRelationship.setVisibility(View.GONE);
        }

        if (!Utils.isEmpty(prisonerModel.fullAddress)) {
            holder.lyAddress.setVisibility(View.VISIBLE);
            holder.txtAddress.setText(String.valueOf(prisonerModel.fullAddress));
        } else {
            holder.lyAddress.setVisibility(View.GONE);
        }

        holder.lyDelete.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (mOnItemClickListener != null) {
                    mOnItemClickListener.onDeleteClick(view, position, prisonerModel);
                }
            }
        });
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return listObject.size();
    }

    @Override
    public long getItemId(int position) {
        return position;
    }
}
